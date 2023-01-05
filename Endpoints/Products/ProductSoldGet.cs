namespace ProdutosApp.Endpoints.Products;

public class ProductSoldGet
{
    //rota
    public static string Template => "/products/sold";
    //metodos de acesso que seram acessados pelo Get
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    //Chama a acao
    public static Delegate Handle => Action;


    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(QueryAllProductsSold query)
    {
        var result = await query.Execute();
               
        return Results.Ok(result);
    }
}
