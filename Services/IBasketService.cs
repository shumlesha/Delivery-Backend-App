using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IBasketService
{
    Task<List<DishInCartDTO>> GetCart(Guid userID);

    Task AddDish(Guid id, Guid userID);
}


public class BasketService : IBasketService
{
    private readonly AppDbContext _context;

    public BasketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DishInCartDTO>> GetCart(Guid userID)
    {


        var dishesInCart = await _context.DishesInCart
            .Include(dishView => dishView.Dish)  
            .Where(dishView => dishView.OrderId == null && dishView.UserId == userID)
            .ToListAsync();

        return dishesInCart.Select(dishView =>
            new DishInCartDTO
            {
                Id = dishView.DishId,
                Name = dishView.Dish.Name,
                Price = dishView.Dish.Price,
                TotalPrice = dishView.Count * dishView.Dish.Price,
                Amount = dishView.Count,
                Image = dishView.Dish.Image
            }).ToList();
    }

    public async Task AddDish(Guid id, Guid userID)
    {
        var dishInCart = await _context.DishesInCart.FirstOrDefaultAsync(
            dishView => dishView.DishId == id && dishView.UserId == userID
                                              && dishView.OrderId == null);

        if (dishInCart != null)
        {
            dishInCart.Count += 1;
        }
        else
        {
            await _context.DishesInCart.AddAsync(
            
                new DishInCart
                {
                    DishId = id,
                    UserId = userID,
                    Count = 1
                }
            );
        }

        await _context.SaveChangesAsync();
    }
}