using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interface;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Talabat.APIs.Controllers;

public class AccountsController : BaseApiController
{
    private readonly UserManager<AppUser> _userManger;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountsController(UserManager<AppUser> userManger , IMapper mapper ,SignInManager<AppUser> signInManager ,ITokenService tokenService )
    {
        _userManger = userManger;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    // Login
    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto model)
    {
        var user = await _userManger.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized(new ApiRequest(401));

        var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
        if (!Result.Succeeded) return Unauthorized(new ApiRequest(401));

        var ReturndUser = new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user, _userManger),

        };
        return Ok(ReturndUser);
    }
    [Authorize]
    [HttpGet ("GetCurrentUser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email);
        var user = await _userManger.FindByEmailAsync(userEmail.Value);
        var ReturndUser = new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user, _userManger),
        };
        return Ok(ReturndUser);
    }
    [Authorize]
    [HttpGet("GetCurrentUserAddress")]
    public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
    {
        //var user = await _userManger.FindUserwithAddressAsync(User);
        //var UserAddress = _mapper.Map<AddressDto>(user.Address);
        //return Ok(UserAddress);
        var user = await _userManger.FindUserwithAddressAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }


        if (user.Address is null)
        {
            return NotFound("This user does not have an address yet.");
        }

        var userAddress = _mapper.Map<AddressDto>(user.Address);

        return Ok(userAddress);
    }

    [Authorize]
    [HttpPut("UpdateCurrentUserAddress")]
    public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto model)
    {
        var user = await _userManger.FindUserwithAddressAsync(User);
        
        var userAdress = _mapper.Map<AddressDto, Address>(model);
        userAdress.AppUserId = user.Id;
        user.Address = userAdress;

        var Result = await _userManger.UpdateAsync(user);
        if (!Result.Succeeded) return BadRequest(new ApiRequest(400));

        return Ok(model);



    }
    // Registeration
    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
        var existingUser = await _userManger.FindByEmailAsync(model.Email);

        if (existingUser is not null)
        {
            return BadRequest(
                new ApiRequest(400, "Email Is Already Exists"));
        }

        var user = _mapper.Map<AppUser>(model);
        user.UserName = model.Email.Split('@')[0];

        var Result =   await _userManger.CreateAsync(user,model.Password);

        //if(!Result.Succeeded) return BadRequest(new ApiRequest(400));

        if (!Result.Succeeded)
        {
            return BadRequest(Result.Errors.Select(error => error.Description));
        }
        //var ReturndUser = _mapper.Map<UserDto>(user);
        var ReturndUser = new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user,_userManger),
        };
        return Ok(ReturndUser);
    }



}
