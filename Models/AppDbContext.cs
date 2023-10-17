using Microsoft.EntityFrameworkCore;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<AddressElement> AddressElements { get; set; }
    public DbSet<DishInCart> DishesInCart { get; set; }
    public DbSet<Hierarchy> Hierarchies { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
}