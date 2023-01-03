using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public static IResult Action(LoginRequest loginRequest,IConfiguration configuration,UserManager<IdentityUser> userManager) //gerando o token
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

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, loginRequest.Email), //criando um claim espeficio de email
                new Claim(ClaimTypes.NameIdentifier, user.Id), //buscando o code do usuário (claim value)
             
            });
        subject.AddClaims(claims); //adicionando o claim a lista de usuarios



        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
