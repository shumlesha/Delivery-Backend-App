using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Exceptions;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IBasketService
{
    Task<List<DishInCartDTO>> GetCart(Guid userID);

    Task AddDish(Guid id, Guid userID);

    Task RemoveDish(Guid dishId, Guid userID, bool increase);
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
            if (await _context.Dishes.FindAsync(id) == null)
            {
                throw new DishNotFoundException("Dish not found");
            }
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

    public async Task RemoveDish(Guid dishId, Guid userID, bool increase)
    {
        if (await _context.Dishes.FindAsync(dishId) == null)
        {
            throw new DishNotFoundException("Dish not found");
        }
        var dishInCart = await _context.DishesInCart.FirstOrDefaultAsync(dishView =>
            dishView.DishId == dishId && dishView.UserId == userID && dishView.OrderId == null);

        if (increase && dishInCart.Count > 1)
        {
            dishInCart.Count -= 1;
        }
        else
        {
            _context.DishesInCart.Remove(dishInCart);
        }

        await _context.SaveChangesAsync();
    }
}