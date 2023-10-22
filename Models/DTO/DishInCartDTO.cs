using System;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishInCartDTO
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid DishId { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    public string Name { get; set; }
    
    public double Price { get; set; }
    
    public double TotalPrice { get; set; }
    
    public string Image { get; set; }
}