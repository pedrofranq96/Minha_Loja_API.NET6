using ProdutosApp.Domain.Users;

namespace ProdutosApp.Endpoints.Clients;
public class ClientPost
{
    //rota
    public static string Template => "/clients";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;
    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, UserCreator userCreator)
    {
        var userClaims = new List<Claim>
        {

             new Claim("Name", clientRequest.Name),
             new Claim("CPF", clientRequest.CPF),

        };
        (IdentityResult identity, string userId) result = await userCreator.Create(clientRequest.Email, clientRequest.Password, userClaims);
        if (!result.identity.Succeeded)
        {
            return Results.ValidationProblem(result.identity.Errors.ConvertProblemDetails());
        }
        
        return Results.Created($"/clients/{result.userId}", result.userId);
    }
}
