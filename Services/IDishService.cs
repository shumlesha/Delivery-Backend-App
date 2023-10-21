using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;
using System.Linq;
namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IDishService
{
    DishPagedListDTO GetListOfDishes(List<Category> categories, bool vegetarian, DishSorting? sorting, int page);
    
    bool RateDish(Guid id, int ratingScore, Guid userID);
}

public class DishService: IDishService
{
    private readonly AppDbContext _context;

    public DishService(AppDbContext context)
    {
        _context = context;
    }

    public DishPagedListDTO GetListOfDishes(List<Category> categories, bool vegetarian, DishSorting? sorting, int page)
    {
        IQueryable<Dish> query = _context.Dishes;
        
        if (categories != null && categories.Count > 0)
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
        
        int pageSize = 10;
        var dishes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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




    public bool RateDish(Guid id, int ratingScore, Guid userID)
    {
        var allUserCarts = _context.Orders.Where(order =>
            order.UserId == userID).SelectMany(order => order.DishesInCarts).ToList();

        var isOrdered = allUserCarts.Any(dishInCart => dishInCart.DishId == id);

        if (!isOrdered)
        {
            return false;
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
        
         
        var dish = _context.Dishes.FirstOrDefault(
            dish => dish.Id == id);

        if (dish != null)
        {
            dish.Rating = _context.Ratings.Where(rating => rating.DishId == id).Average(rating => rating.Value);
            
        }
        
        
        _context.SaveChanges();

        return true;
    }
}