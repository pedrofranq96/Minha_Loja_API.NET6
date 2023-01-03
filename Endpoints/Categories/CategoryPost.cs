namespace ProdutosApp.Endpoints.Categories;

public class CategoryPost
{
    //rota
    public static string Template => "/categories";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy ="EmployeePolicy")]
    public static async Task<IResult> Action(CategoryRequest categoryRequest,HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value; //busca o Id do usuario na claim ao autenticar
        var category = new Category(categoryRequest.Name, userId, userId);
        

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());
        }
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
