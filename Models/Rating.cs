using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Rating
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public double Value { get; set; }
    
    public Guid DishId { get; set; }
    public Dish Dish { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
}