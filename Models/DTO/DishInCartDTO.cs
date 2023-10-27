using System;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishInCartDTO
{
    public Guid Id { get; set; }
    
  
    public Guid DishId { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public double TotalPrice { get; set; }
    
    public string Image { get; set; }
}