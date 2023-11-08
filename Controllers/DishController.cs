using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_1.Exceptions;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;

[ApiController]
[Route("api/dish")]
public class DishController: ControllerBase
{
    private IDishService _dishService;
    
    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }
    
    /// <summary>
    /// Get a list of dishes (menu)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishPagedListDTO>> GetListOfDishes([FromQuery] List<Category> categories, bool vegetarian = false,
        DishSorting? sorting = null, int page = 1)
    {
        try
        {
            return await _dishService.GetListOfDishes(categories, vegetarian, sorting, page);
        }
        catch (WrongPageNumException)
        {
            return NotFound(new Response { status = "404", message = "Wrong page number" });

        }
        catch (Exception e)
        {
            return BadRequest(new Response { message = e.Message });
        }
        
    }


    /// <summary>
    /// Get information about concrete dish
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<DishDTO>> GetDish(Guid id)
    {
        try
        {
            return await _dishService.GetDish(id);
        }
        catch (DishNotFoundException)
        {
            return NotFound(new Response { status = "404", message = "Dish not found"});
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
    }
    
    
    /// <summary>
    /// Checks if user is able to set rating of the dish
    /// </summary>
    [Authorize]
    [HttpGet("{id}/rating/check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> CheckRatePossibility(Guid id)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);


            return Ok(await _dishService.CheckRatePossibility(id, userID));
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
    /// Set a rating for a dish
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpPost("{id}/rating")]
    public async Task<IActionResult> RateDish(Guid id, int ratingScore)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            bool israted = await _dishService.RateDish(id, ratingScore, userID);

            return Ok();
        }
        catch (NoOrdersException e)
        {
            return NotFound(new Response { status = "404", message = e.Message });
        }
        catch (DishNotFoundException e)
        {
            return NotFound(new Response { status = "404", message = e.Message });
        }
        catch (NoOrderedDishException e)
        {
            return NotFound(new Response { status = "404", message = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
        
    }

}