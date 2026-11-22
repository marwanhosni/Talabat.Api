using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Interface;
public interface ITokenService
{
    public Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager);
}
