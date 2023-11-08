using System;
using System.Collections.Generic;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Exceptions;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IDishService
{
    Task<DishPagedListDTO> GetListOfDishes(List<Category> categories, bool vegetarian, DishSorting? sorting, int page);

    Task<DishDTO> GetDish(Guid id);
    Task<bool> CheckRatePossibility(Guid id, Guid userID);
    
    Task<bool> RateDish(Guid id, int ratingScore, Guid userID);

}

public class DishService: IDishService
{
    private readonly AppDbContext _context;

    public DishService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DishPagedListDTO> GetListOfDishes(List<Category> categories, bool vegetarian, DishSorting? sorting, int page)
    {
        if (page < 1)
        {
            throw new WrongPageNumException("Page number is less than 1!");
        }
        
        IQueryable<Dish> query = _context.Dishes;
        
        if (categories.Count > 0)
        {
            query = query.Where(dish => categories.Contains(dish.Category));
        }
        
        if (vegetarian)
        {
            query = query.Where(dish => dish.Vegeterian == true);
        }

        switch (sorting)
        {
            case DishSorting.NameAsc:
                query = query.OrderBy(dish => dish.Name);
                break;
            case DishSorting.NameDesc:
                query = query.OrderByDescending(dish => dish.Name);
                break;
            case DishSorting.PriceAsc:
                query = query.OrderBy(dish => dish.Price);
                break;
            case DishSorting.PriceDesc:
                query = query.OrderByDescending(dish => dish.Price);
                break;
            case DishSorting.RatingAsc:
                query = query.OrderBy(dish => dish.Rating);
                break;
            case DishSorting.RatingDesc:
                query = query.OrderByDescending(dish => dish.Rating);
                break;
            default:
                break;
        }
        
        int pageSize = 6;
        var dishes = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var dishToDTO = dishes.Select(dish =>
            new DishDTO
            {
                id = dish.Id,
                name = dish.Name,
                desctiption = dish.Description,
                price = dish.Price,
                image = dish.Image,
                vegeterian = dish.Vegeterian,
                rating = dish.Rating,
                category = dish.Category
            }
        ).ToList();

        return new DishPagedListDTO
        {
            Dishes = dishToDTO,
            Pagination = new PageInfoModel
            {
                Size = dishToDTO.Count,
                Count = query.Count(),
                Current = page
            }
        };


    }


    public async Task<DishDTO> GetDish(Guid id)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(dish =>
            dish.Id == id);

        if (dish == null)
        {
            throw new DishNotFoundException("Dish not found");
        }

        return new DishDTO
        {
            id = dish.Id,
            name = dish.Name,
            desctiption = dish.Description,
            price = dish.Price,
            image = dish.Image,
            vegeterian = dish.Vegeterian,
            rating = dish.Rating,
            category = dish.Category
        };
    }

    public async Task<bool> CheckRatePossibility(Guid id, Guid userID)
    {
        
        var dish = _context.Dishes.SingleOrDefault(dish => dish.Id == id);
        
        if (dish == null)
        {
            throw new DishNotFoundException($"Dish not found");
        }
        
        var allUserCarts = await _context.Orders.Where(order =>
            order.UserId == userID).SelectMany(order => order.DishesInCarts).ToListAsync();
        
        return allUserCarts.Any(dishInCart => dishInCart.DishId == id);
        
    }
    
    public async Task<bool> RateDish(Guid id, int ratingScore, Guid userID)
    {
        var allUserCarts = _context.Orders.Where(order =>
            order.UserId == userID).SelectMany(order => order.DishesInCarts).ToList();

        if (allUserCarts == null)
        {
            throw new NoOrdersException("User doesn't have ordered dishes!");
        }
        
        var isOrdered = allUserCarts.Any(dishInCart => dishInCart.DishId == id);

        if (!isOrdered)
        {
            throw new NoOrderedDishException("User didn't order this dish");
        }

        var RatedBefore = _context.Ratings.FirstOrDefault(rating => rating.DishId == id
                                                                      && rating.UserId == userID);
        if (RatedBefore != null)
        {
            RatedBefore.Value = ratingScore;
        }
        else
        {
            var settedRating = new Rating
            {
                Id = Guid.NewGuid(),
                Value = ratingScore,
                DishId = id,
                UserId = userID
            };
            _context.Ratings.Add(settedRating);
            
        }
        await _context.SaveChangesAsync();
         
        var dish = _context.Dishes.FirstOrDefault(
            dish => dish.Id == id);

        if (dish != null)
        {
            dish.Rating = _context.Ratings.Where(rating => rating.DishId == id).Average(rating => rating.Value);
            
        }
        else
        {
            throw new DishNotFoundException("Dish not found");
        }
        
        await _context.SaveChangesAsync();

        return true;
    }

}