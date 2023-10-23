﻿using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IOrderService
{
    Task<OrderDTO> GetOrder(Guid id);
    Task<List<OrderInfoDTO>> GetOrdersList(Guid userID);
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
            .FirstOrDefaultAsync(order => order.Id == id);

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
                    Name = dishView.Name,
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
}