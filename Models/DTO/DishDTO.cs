using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishDTO
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    public string Description { get; set; }
    
    public bool IsVegeterian { get; set; }
    
    public string Photo { get; set; }
    
    public double? RateValue { get; set; }
    
    public Category Category  { get; set; }
    
}