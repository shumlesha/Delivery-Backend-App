using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class OrderDTO
{
    public Guid Id { get; set; }
    
    [Required]
    public DateTime DeliveryTime { get; set; }
    
    [Required]
    public DateTime OrderTime { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public List<DishInCartDTO> Dishes  { get; set; }
    
    [Required]
    public string Address { get; set; }
}