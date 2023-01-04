using Microsoft.AspNetCore.Identity;
using ProdutosApp.Endpoints;
using ProdutosApp.Endpoints.Clients;

namespace ProdutosApp.Domain.Users;

public class UserCreator
{
    private readonly UserManager<IdentityUser> _userManager;
    public UserCreator(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<(IdentityResult, string)> Create(string email,string password, List<Claim> claims) 
    {
        var newUser = new IdentityUser { UserName = email, Email = email }; //criando o cliente

        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            return (result, String.Empty);
        }

        return (await _userManager.AddClaimsAsync(newUser, claims), newUser.Id); 

    }
}
