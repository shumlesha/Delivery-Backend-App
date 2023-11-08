using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IOrderService
{
    Task<OrderDTO> GetOrder(Guid id);
    Task<List<OrderInfoDTO>> GetOrdersList(Guid userID);

    Task MakeOrder(Guid userID, OrderCreateDTO orderCreateDTO);

    Task ConfirmOrder(Guid id);
}   

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDTO> GetOrder(Guid id)
    {
        var queriedOrder = await _context.Orders.Include(order => order.DishesInCarts)
            .ThenInclude(dishInCart => dishInCart.Dish)
            .FirstOrDefaultAsync(order => order.Id == id);

        if (queriedOrder == null)
        {
            throw new Exception("No order with such id!");
        }
            
        return new OrderDTO
        {
            Id = queriedOrder.Id,
            DeliveryTime = queriedOrder.DeliveryTime,
            OrderTime = queriedOrder.OrderTime,
            Status = queriedOrder.Status,
            Price = queriedOrder.Price,
            Dishes = queriedOrder.DishesInCarts
                .Select(dishView => new DishInCartDTO
                {
                    Id = dishView.DishId,
                    Name = dishView.Dish.Name,
                    Price = dishView.Dish.Price,
                    TotalPrice = dishView.Count * dishView.Dish.Price,
                    Amount = dishView.Count,
                    Image = dishView.Dish.Image
                    
                }).ToList(),
            Address = queriedOrder.Address
        };
    }

    public async Task<List<OrderInfoDTO>> GetOrdersList(Guid userID)
    {
        var allUserOrders = await _context.Orders.Where(order => order.UserId == userID).ToListAsync();

        return allUserOrders.Select(
            order => new OrderInfoDTO
            {
                id = order.Id,
                deliveryTime = order.DeliveryTime,
                orderTime = order.OrderTime,
                status = order.Status,
                price = order.Price

            }).ToList();
    }

    public async Task MakeOrder(Guid userID, OrderCreateDTO orderCreateDTO)
    {
        var userWantedDishes = await _context.DishesInCart.Where(dishView => dishView.UserId == userID &&
                                                                             dishView.OrderId == null)
            .Include(dishInCart => dishInCart.Dish).ToListAsync();

        if (userWantedDishes == null || userWantedDishes.Count == 0)
        {
            throw new Exception("No dishes in cart!");
        }
        
        
        var neworderid = Guid.NewGuid();
        _context.Orders.Add(
            new Order
            {
                Id = neworderid,
                DeliveryTime = orderCreateDTO.deliveryTime,
                OrderTime = DateTime.UtcNow,
                Price = userWantedDishes.Sum(dishView => dishView.Count * dishView.Dish.Price),
                Address = orderCreateDTO.addressId.ToString(),
                Status = Status.InProcess,
                UserId = userID
            });

        await _context.SaveChangesAsync();

        foreach (var dishView in userWantedDishes)
        {
            dishView.OrderId = neworderid;
        }

        await _context.SaveChangesAsync();
    }


    public async Task ConfirmOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            throw new Exception("No order with such id!");
        }

        if (order.Status != Status.InProcess)
        {
            throw new Exception("This order is already delivered!");
        }
        
        order.Status = Status.Delivered;
        _context.Update(order);

        await _context.SaveChangesAsync();
    }
}