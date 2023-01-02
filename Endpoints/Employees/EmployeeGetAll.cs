using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace ProdutosApp.Endpoints.Employees;

public class EmployeeGetAll
{
    //rota
    public static string Template => "/employees";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
    {
        var db = new SqlConnection(configuration["ConnectionString:ProdutosApi"]); // Consulta feita via Dapper para melhor performace. Metodo de paginacao no banco de dados no final da query
        var query =
            @"select Email, ClaimValue as Name
              from AspNetUsers u INNER JOIN AspNetUserClaims c
              on u.id = c.UserId AND claimtype = 'Name'
              order by name
              OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";


        var employees = db.Query<EmployeeResponse>(
            query,
            new { page , rows }); //objeto anonimo 
            

        return Results.Ok(employees);






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
