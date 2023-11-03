using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_1.Exceptions;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController: ControllerBase
{
    private IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }
    
    /// <summary>
    /// Get user cart
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<DishInCartDTO>>> GetCart()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
        var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

        return Ok(await _basketService.GetCart(userID));
    }
    
    
    /// <summary>
    /// Add dish to cart
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPost("dish/{dishId}")]
    public async Task<IActionResult> AddDish(Guid dishId)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            await _basketService.AddDish(dishId, userID);

            return Ok();
        }
        catch (DishNotFoundException e)
        {
            return NotFound(new Response { status = "404", message = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
        
    }
    
    
    /// <summary>
    /// Decrease the number of dishes in the cart(if increase = true), or remove the dish completely(increase = false)
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpDelete("dish/{dishId}")]
    public async Task<IActionResult> RemoveDish(Guid dishId, bool increase = false)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            await _basketService.RemoveDish(dishId, userID, increase);

            return Ok();
        }
        catch (DishNotFoundException e)
        {
            return NotFound(new Response { status = "404", message = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
    }
    
}