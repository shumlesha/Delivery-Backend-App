using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;

[ApiController]
[Route("api/account")]
public class UserAccountController: ControllerBase
{
    private IUserAccountService _userAccountService;

    public UserAccountController(IUserAccountService userAccount)
    {
        _userAccountService = userAccount;
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> UserRegister(UserRegisterModel userRegisterModel)
    {
        try
        {
            var userTOKEN = await _userAccountService.UserRegister(userRegisterModel);
            return Ok(new {
                token = userTOKEN
            });
        }
        catch
        {
            return BadRequest();
        }
        
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin(LoginCredentials loginCredentials)
    {

        try
        {
            var userTOKEN = await _userAccountService.UserLogin(loginCredentials);
            return Ok(new { token = userTOKEN });

        }
        catch
        {
            return BadRequest();
        }
        
       
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> UserLogoutProfile()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _userAccountService.UserLogoutProfile(token);
        return Ok();
    }
    
    
    
    [Authorize]
    [HttpGet("profile")]
    public async Task<UserDTO> UserGetProfile()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;

        return await _userAccountService.UserGetProfile(new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UserEditProfile(UserEditModel userEditModel)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
        var guid = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
        await _userAccountService.UserEditProfile(userEditModel, guid);

        return Ok();
    }
}