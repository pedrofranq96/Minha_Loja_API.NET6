using Microsoft.AspNetCore.Authorization;
using ProdutosApp.Infra.Data;

namespace ProdutosApp.Endpoints.Employees;

public class EmployeeGetAll
{
    //rota
    public static string Template => "/employees";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
           return Results.Ok(query.Execute(page.Value,rows.Value));
        //--- Consulta para buscar usuário e email via Entity Framework
        //var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList(); //skip = quantidade de linhas /take = regra para retorno de linhas
        //var employees = new List<EmployeeResponse>();
        //foreach (var item in users)
        //{
        //    var claims = userManager.GetClaimsAsync(item).Result;
        //    var claimName = claims.FirstOrDefault(c => c.Type == "Name"); // consulta para retornar o nome do usuario 
        //    var userName = claimName != null ? claimName.Value : string.Empty; //verifica se o nome é nulo, se for retorna vazio.
        //    employees.Add(new EmployeeResponse(item.Email, userName));
        //}
        //return Results.Ok(employees);
    }
}
