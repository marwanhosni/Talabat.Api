using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;
using System.Security.Claims;

namespace Talabat.APIs.Extensions;

public static class UserMangerExtensions
{
    public static async Task<AppUser?> FindUserwithAddressAsync(this UserManager<AppUser> userManger, ClaimsPrincipal User)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await userManger.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

}
