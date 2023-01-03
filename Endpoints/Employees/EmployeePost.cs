using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
    public static IResult Action(EmployeeRequest employeeRequest,HttpContext http, UserManager<IdentityUser> userManager)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var newUser = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email }; //criando o usuario

        var result = userManager.CreateAsync(newUser, employeeRequest.Password).Result;

        if (!result.Succeeded)
        {
            return Results.ValidationProblem(result.Errors.ConvertProblemDetails());
        }


        var userClaims = new List<Claim>
        {
             new Claim("EmployeeCode", employeeRequest.EmployeeCode),
             new Claim("Name", employeeRequest.Name),
             new Claim("CreatedBy", userId)
        };

        
        var claimResult = userManager.AddClaimsAsync(newUser,userClaims).Result; //lista de claims 
        

        if (!claimResult.Succeeded)
        {
            return Results.ValidationProblem(result.Errors.ConvertProblemDetails());
        }

        

        return Results.Created($"/employees/{newUser.Id}", newUser.Id);
    }
}
