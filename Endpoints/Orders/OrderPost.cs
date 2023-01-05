using ProdutosApp.Domain.Orders;
using ProdutosApp.Domain.Users;

namespace ProdutosApp.Endpoints.Orders;
public class OrderPost
{
    //rota
    public static string Template => "/orders";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;
    [Authorize(Policy = "CPFPolicy")]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http, ApplicationDbContext context)
    {
        var clientId = http.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientName = http.User.Claims
            .First(c => c.Type == "Name").Value;

        List<Product> productsFound = null;
        if (orderRequest.ProductsId != null && orderRequest.ProductsId.Any())
            productsFound = context.Products.Where(p => orderRequest.ProductsId.Contains(p.Id)).ToList();

        var order = new Order(clientId, clientName, productsFound, orderRequest.DeliveryAddress);
        if (!order.IsValid)
        {
            return Results.ValidationProblem(order.Notifications.ConvertProblemDetails());
        }
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}
