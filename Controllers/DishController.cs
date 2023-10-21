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


    [HttpGet("{id}")]
    public ActionResult<DishDTO> GetDish(Guid id)
    {
        return _dishService.GetDish(id);
    }
    
}