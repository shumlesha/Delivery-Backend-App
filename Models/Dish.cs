using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Dish
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public bool Vegeterian { get; set; }
    
    public double Rating { get; set; }
  
    public string Image { get; set; }
    
    public Category Category { get; set; }
    
    public List<Rating> Ratings { get; set; }
    public List<DishInCart> DishesInCart { get; set; }
}