using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
    public async Task<IActionResult> UserRegister(UserDTO userDTO)
    {
        try
        {
            var userTOKEN = await _userAccountService.UserRegister(userDTO);
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
    public async Task<IActionResult> UserLogin(UserDTO userDTO)
    {

        try
        {
            var userTOKEN = await _userAccountService.UserLogin(userDTO);
            return Ok(new { token = userTOKEN });

        }
        catch
        {
            return BadRequest();
        }
        
       
    }
}