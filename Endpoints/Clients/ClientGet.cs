using ProdutosApp.Domain.Users;

namespace ProdutosApp.Endpoints.Clients;
public class ClientGet
{
    //rota
    public static string Template => "/clients";

    //metodos de acesso que seram acessados pelo GET
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;
    [AllowAnonymous]
    public static async Task<IResult> Action(HttpContext http)
    {
        var user = http.User;
        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(c => c.Type == "Name").Value,
            CPF = user.Claims.First(c => c.Type == "CPF").Value,

        };
        return Results.Ok(result);
    }
}
