using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    [HttpGet]
    public ActionResult<DishPagedListDTO> GetListOfDishes([FromQuery] List<Category> categories, bool vegetarian = false,
        DishSorting? sorting = null, int page = 1)
    {
        return _dishService.GetListOfDishes(categories, vegetarian, sorting, page);
    }



    [HttpGet("{id}/rating/check")]
    public bool CheckRatePossibility(Guid id)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
        var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

        return _dishService.CheckRatePossibility(id, userID);
    }
}