using Microsoft.EntityFrameworkCore;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
}