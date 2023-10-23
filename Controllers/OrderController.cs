using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;


[ApiController]
[Route("api/order")]
public class OrderController: ControllerBase
{
    private IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(Guid id)
    {
        return Ok(await _orderService.GetOrder(id));

    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderInfoDTO>>> GetOrdersList()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var tokenhandler = new JwtSecurityTokenHandler();
        var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
        var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

        return Ok(await _orderService.GetOrdersList(userID));
    }
}