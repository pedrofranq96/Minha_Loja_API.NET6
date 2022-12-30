using Microsoft.AspNetCore.Mvc;
using ProdutosApp.Domain.Products;
using ProdutosApp.Infra.Data;

namespace ProdutosApp.Endpoints.Categories;

public class CategoryPut
{
    //rota
    public static string Template => "/categories/{id}";

    //metodos de acesso que seram acessados pelo Put
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute]Guid id,CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
        category.Name = categoryRequest.Name;
        category.Active = categoryRequest.Active;
        context.SaveChanges();

        return Results.Ok();
    }
}
