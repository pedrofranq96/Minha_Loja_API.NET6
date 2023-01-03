using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdutosApp.Domain.Products;
using ProdutosApp.Infra.Data;
using static System.Net.WebRequestMethods;
using System.Security.Claims;

namespace ProdutosApp.Endpoints.Categories;

public class CategoryPut
{
    //rota
    public static string Template => "/categories/{id:guid}";

    //metodos de acesso que seram acessados pelo Put
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolice")]
    public static IResult Action([FromRoute]Guid id,CategoryRequest categoryRequest,HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null)
        {
            return Results.NotFound();
        }
        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);
        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertProblemDetails());
        }
        
        context.SaveChanges();

        return Results.Ok();
    }
}
