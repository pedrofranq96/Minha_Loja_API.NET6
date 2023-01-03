namespace ProdutosApp.Endpoints.Categories;

public class CategoryGetAll
{
    //rota
    public static string Template => "/categories";

    //metodos de acesso que seram acessados pelo Get
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories.ToList();
        var response = categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Active));

        return Results.Ok(response);
    }
}
