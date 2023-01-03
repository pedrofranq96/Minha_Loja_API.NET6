using Microsoft.AspNetCore.Authorization;
using ProdutosApp.Domain.Products;
using ProdutosApp.Infra.Data;

namespace ProdutosApp.Endpoints.Categories;

public class CategoryPost
{
    //rota
    public static string Template => "/categories";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize]
    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {

        var category = new Category(categoryRequest.Name, "Test", "Test");
        

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());
        }
        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
