using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProdutosApp.Endpoints.Security;

public class TokenPost
{
    //rota
    public static string Template => "/token";

    //metodos de acesso que seram acessados pelo POST
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    //Chama a acao
    public static Delegate Handle => Action;

    public static IResult Action(LoginRequest loginRequest,UserManager<IdentityUser> userManager) //gerando o token
    {
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if (user == null)
        {
            Results.BadRequest();
        }
        if (!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
        {
            Results.BadRequest();
        }

        var key = Encoding.ASCII.GetBytes("A@fderwfQQSDXCCer34");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, loginRequest.Email),
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "ProdutosApp",
            Issuer = "Issuer"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
