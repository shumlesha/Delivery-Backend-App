using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<DishPagedListDTO> GetListOfDishes([FromQuery] List<Category> categories, bool vegetarian = false,
        DishSorting? sorting = null, int page = 1)
    {
        return _dishService.GetListOfDishes(categories, vegetarian, sorting, page);
    }


    /// <summary>
    /// Get information about concrete dish
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [HttpGet("{id}")]
    public ActionResult<DishDTO> GetDish(Guid id)
    {
        try
        {
            return _dishService.GetDish(id);
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
    public bool CheckRatePossibility(Guid id)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
        var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);


        return _dishService.CheckRatePossibility(id, userID);
    }
    
    
    /// <summary>
    /// Set a rating for a dish
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost("{id}/rating")]
    public IActionResult RateDish(Guid id, int ratingScore)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        
            bool israted = _dishService.RateDish(id, ratingScore, userID);

            if (israted)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
        
    }

}