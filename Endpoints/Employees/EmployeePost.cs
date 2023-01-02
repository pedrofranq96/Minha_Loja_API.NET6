using Microsoft.AspNetCore.Identity;
using ProdutosApp.Domain.Products;
using ProdutosApp.Infra.Data;
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

    public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email }; //criando o usuario

        var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors.First());
        }


        var userClaims = new List<Claim>
        {
             new Claim("EmployeeCode", employeeRequest.EmployeeCode),
             new Claim("Name", employeeRequest.Name)
        };

        
        var claimResult = userManager.AddClaimsAsync(user,userClaims).Result; //lista de claims 
        

        if (!claimResult.Succeeded)
        {
            return Results.BadRequest(claimResult.Errors.First());
        }

        

        return Results.Created($"/employees/{user.Id}", user.Id);
    }
}
