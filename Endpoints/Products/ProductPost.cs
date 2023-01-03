namespace ProdutosApp.Endpoints.Products;

public class ProductPost
{
    //rota
    public static string Template => "/products";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequest productRequest, HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value; //busca o Id do usuario na claim ao autenticar
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == productRequest.CategoryId);
        var product = new Product(productRequest.Name, category, productRequest.Description, productRequest.HasStock,productRequest.Price, userId);


        if (!product.IsValid)
        {
            return Results.ValidationProblem(product.Notifications.ConvertProblemDetails());
        }
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product.Id);
    }
}
