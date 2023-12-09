using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using serveu.Models;

namespace ServeU.Utils;


public class AuthUtils
{


    private readonly UserManager<ApplicationUser> userManager;
    public AuthUtils(UserManager<ApplicationUser> _userManager)
    {
        userManager = _userManager;
    }

    public async Task<ApplicationUser> getAuthenticatedUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        var userName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
        var user = await userManager.FindByNameAsync(userName);

        return user;

    }
}