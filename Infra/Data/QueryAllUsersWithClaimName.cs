using Dapper;
using Microsoft.Data.SqlClient;
using ProdutosApp.Endpoints.Employees;

namespace ProdutosApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;
    public QueryAllUsersWithClaimName(IConfiguration configuration)
	{
        this.configuration = configuration;
    }

    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:ProdutosApi"]); // Consulta feita via Dapper para melhor performace. Metodo de paginacao no banco de dados no final da query
        var query =
            @"select Email, ClaimValue as Name
              from AspNetUsers u INNER JOIN AspNetUserClaims c
              on u.id = c.UserId AND claimtype = 'Name'
              order by name
              OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";


        return await db.QueryAsync<EmployeeResponse>(
            query,
            new { page, rows }); //objeto anonimo 
    }

    
}
