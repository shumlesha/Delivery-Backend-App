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
    
    public bool IsVegeterian { get; set; }
    
  
    public string Photo { get; set; }
    
    public Category Category { get; set; }
    

}