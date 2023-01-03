namespace ProdutosApp.Endpoints.Security;

public class TokenPost
{
    //rota
    public static string Template => "/token";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;
    [AllowAnonymous]
    public static async Task<IResult> Action(
        LoginRequest loginRequest,IConfiguration configuration,UserManager<IdentityUser> userManager,ILogger<TokenPost> log, 
        IWebHostEnvironment environment) //gerando o token
    {
        log.LogInformation("Getting token");
       

        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            Results.BadRequest();
        }
        if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            Results.BadRequest();
        }

        var claims = await userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, loginRequest.Email), //criando um claim espeficio de email
                new Claim(ClaimTypes.NameIdentifier, user.Id), //buscando o code do usuário (claim value)
             
            });
        subject.AddClaims(claims); //adicionando o claim a lista de usuarios



        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = environment.IsDevelopment() || environment.IsStaging() ?
            DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.AddMinutes(2) //1 ano se for em development - 2 minutos se for em homologacao (staging) 
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
