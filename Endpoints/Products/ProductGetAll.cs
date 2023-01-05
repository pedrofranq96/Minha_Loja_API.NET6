namespace ProdutosApp.Endpoints.Products;

public class ProductGetAll
{
    //rota
    public static string Template => "/products";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var results = products.Select(p => new ProductResponse(p.Id,p.Name, p.Category.Name, p.Description, p.HasStock,p.Price, p.Active));
        return Results.Ok(results);
    }
}
