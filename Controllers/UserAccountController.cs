using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using webNET_Hits_backend_aspnet_project_1.Exceptions;
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
    
    /// <summary>
    /// Register new user
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status409Conflict)]
    [HttpPost("register")]
    public async Task<IActionResult> UserRegister(UserRegisterModel userRegisterModel)
    {
        try
        {
            var userTOKEN = await _userAccountService.UserRegister(userRegisterModel);
            return Ok(new
            {
                token = userTOKEN
            });
        }
        catch (ArgumentNullException)
        {
            return BadRequest(new Response { status = "400", message = "Entered null data" });
        }
        catch (UserAlreadyExistsException)
        {
            return Conflict(new Response { status = "409" , message = "User already exists"});
        }
        catch (DbUpdateException)
        {
            return BadRequest(new Response { status = "400", message = "User registration failed" });
        }
        catch (Exception e)
        {
            return BadRequest(new Response {status = "400", message = e.Message });
        }
        
        
    }
    
    /// <summary>
    /// Log in to the system
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> UserLogin(LoginCredentials loginCredentials)
    {

        try
        {
            var userTOKEN = await _userAccountService.UserLogin(loginCredentials);
            return Ok(new { token = userTOKEN });

        }
        catch (ArgumentNullException)
        {
            return BadRequest(new Response { status = "400", message = "Entered null data" });
        }
        catch (UserNotFoundException)
        {
            return NotFound(new Response { status = "404", message = "User not found" });
        }
        catch (InvalidPasswordException)
        {
            return Unauthorized(new Response { status = "401", message = "User entered invalid password!" });
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status="400", message = e.Message });
        }
        
       
    }
    
    /// <summary>
    /// Log out system user
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> UserLogoutProfile()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _userAccountService.UserLogoutProfile(token);
        return Ok();
    }
    
    
    /// <summary>
    /// Get user profile
    /// </summary>
    [Authorize]
    [HttpGet("profile")]
    public async Task<UserDTO> UserGetProfile()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;

        return await _userAccountService.UserGetProfile(new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value));
    }

    
    /// <summary>
    /// Edit user Profile
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UserEditProfile(UserEditModel userEditModel)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var guid = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
            await _userAccountService.UserEditProfile(userEditModel, guid);

            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest(new Response { status = "400", message = "Entered null data" });
        }
        
        catch (Exception e)
        {
            return BadRequest(new Response { message = e.Message });
        }
    }
}