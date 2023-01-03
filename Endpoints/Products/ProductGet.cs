using ProdutosApp.Domain.Products;

namespace ProdutosApp.Endpoints.Products;

public class ProductGet
{
    //rota
    public static string Template => "/products/{id}";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id,ApplicationDbContext context)
    {
        var product = await context.Products.Include(p => p.Category).Where(p => p.Id == id).FirstAsync();

        if (product != null)
        {
            return Results.Ok(product);
        }
        return Results.NotFound();
    }
}
