using Microsoft.AspNetCore.Diagnostics;
using ProdutosApp.Endpoints.Products;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSerilog((context, configuration) =>
{ //configuração para criar o log no banco de dados
    configuration
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
        context.Configuration["ConnectionString:ProdutosApi"],
            sinkOptions: new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = true,
                TableName = "LogAPI"
            });
});

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:ProdutosApi"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => // configuracao de senha para o Identity user
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<ApplicationDbContext>(); //adicionando o identity como serviço do aspnet

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build(); //configuração padrão para restringir acesso a usuarios que não contém o token de autenticacao

    options.AddPolicy("EmployeePolicy", p => //gerando a politica de usuário para dar nivel de permissao
        p.RequireAuthenticatedUser()
        .RequireClaim("EmployeeCode"));


});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters() //Validacoes dos campos do token 
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero, //tempo de validacao
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});
builder.Services.AddScoped<QueryAllUsersWithClaimName>(); //Adicionando a classe como serviço 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle);
app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);
app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handle);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);
app.MapMethods(ProductGet.Template, ProductGet.Methods, ProductGet.Handle);
app.MapMethods(ProductGetShowcase.Template, ProductGetShowcase.Methods, ProductGetShowcase.Handle);

app.UseExceptionHandler("/error"); //ao possuir algum erro de conexão, ou erro de banco, é acionado este endpoint
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;
    if (error != null)
    {
        if (error is SqlException)
        {
            return Results.Problem(title: "Database out", statusCode: 500);
        }
        else if(error is BadHttpRequestException)
        {
            return Results.Problem(title: "Erro de tipagem enviada", statusCode: 500);
        }
    }
    return Results.Problem(title: "An error ocurred", statusCode: 500);
});

app.Run();

