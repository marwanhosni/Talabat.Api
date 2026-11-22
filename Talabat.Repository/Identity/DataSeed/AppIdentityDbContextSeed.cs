using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Identity.DataSeed;
public static class AppIdentityDbContextSeed
{
    public static async Task SeedUSerAsync(UserManager<AppUser> _userManager)
    {
        var user = new AppUser()
        {
            DisplayName = "Marwan Hosny",
            Email = "Marwanhosni785@gmail.com",
            UserName = "Marwan.hosni",
            PhoneNumber = "01212882622",
        };
        await _userManager.CreateAsync(user, "Marwan@123");
    }
}
