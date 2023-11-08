using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;


[ApiController]
[Route("api/order")]
public class OrderController: ControllerBase
{
    private IOrderService _orderService;
    private readonly TimeSpan _orderDiffTimeout;
    public OrderController(IOrderService orderService, IOptions<OrderParams> orderParams)
    {
        _orderService = orderService;
        _orderDiffTimeout = TimeSpan.FromMinutes(orderParams.Value.OrderDifference);
    }
    
    /// <summary>
    /// Get information about concrete order
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(Guid id)
    {
        try
        {
            return Ok(await _orderService.GetOrder(id));
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }

    }

    
    /// <summary>
    /// Get a list of orders
    /// </summary>
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
    
    
    /// <summary>
    /// Creating the order from dishes in basket
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> MakeOrder(OrderCreateDTO orderCreateDTO)
    {
        if (orderCreateDTO.deliveryTime < DateTime.Now + _orderDiffTimeout)
        {
            return BadRequest(new Response {status = "400", message = "Wrong delivery time!"});
        }
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenhandler = new JwtSecurityTokenHandler();
            var normalToken = tokenhandler.ReadToken(token) as JwtSecurityToken;
            var userID = new Guid(normalToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            await _orderService.MakeOrder(userID, orderCreateDTO);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
    }
    
    
    /// <summary>
    /// Confirm order delivery
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [Authorize]
    [HttpPost("{id}/status")]
    public async Task<ActionResult> ConfirmOrder(Guid id)
    {
        try
        {
            await _orderService.ConfirmOrder(id);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new Response { status = "400", message = e.Message });
        }
    }
}