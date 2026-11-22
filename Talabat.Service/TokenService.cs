using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Service;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> _userManager)
    {
        
        //1. Header



        //2.Payload
        //Private Claims (name ,id ,email ,role)
        var AuthClaims  = new List<Claim>
        {
            new Claim(ClaimTypes.GivenName, user.DisplayName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach(var Role in userRoles)
        {
            AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
        }

        //3. Signature
        //3.1. key
        var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: AuthClaims,
            signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.Now.AddDays(7)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
