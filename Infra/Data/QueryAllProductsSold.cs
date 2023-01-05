using ProdutosApp.Endpoints.Products;

namespace ProdutosApp.Infra.Data;
public class QueryAllProductsSold
{
    private readonly IConfiguration configuration;
    public QueryAllProductsSold(IConfiguration configuration)
	{
        this.configuration = configuration;
    }

    public async Task<IEnumerable<ProductsSoldResponse>> Execute()
    {
        var db = new SqlConnection(configuration["ConnectionString:ProdutosApi"]); // Consulta feita via Dapper para melhor performace. Metodo de paginacao no banco de dados no final da query
        var query =
            @"select
                 p.Id,
                 p.Name,
                 COUNT(*) Amount 
            from 
                 Orders o inner join OrderProducts op on o.Id = op.OrdersId
                 inner join Products p on p.Id = op.ProductsId
            group BY
                 p.Id, p.Name
            order By Amount  desc";

        return await db.QueryAsync<ProductsSoldResponse>(query);
           
    }

    
}
