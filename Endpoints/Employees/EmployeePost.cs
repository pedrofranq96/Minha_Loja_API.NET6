using ProdutosApp.Domain.Users;
using ProdutosApp.Endpoints.Clients;

namespace ProdutosApp.Endpoints.Employees;

public class EmployeePost
{
    //rota
    public static string Template => "/employees";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;
    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeRequest,HttpContext http, UserCreator userCreator)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim>
        {
             new Claim("EmployeeCode", employeeRequest.EmployeeCode),
             new Claim("Name", employeeRequest.Name),
             new Claim("CreatedBy", userId)
        };

        (IdentityResult identity, string userId) result = await userCreator.Create(employeeRequest.Email, employeeRequest.Password, userClaims);
        if (!result.identity.Succeeded)
        {
            return Results.ValidationProblem(result.identity.Errors.ConvertProblemDetails());
        }
     
        return Results.Created($"/employees/{result.userId}", result.userId);
    }
}
