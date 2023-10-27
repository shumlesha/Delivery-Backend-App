using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public DateTime OrderTime { get; set; }
    
    public double Price { get; set; }
    
    public Guid? AddressId { get; set; }
    
    public Status Status { get; set; }
    
    public List<DishInCart> DishesInCarts { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set;  }
    
    public string Address { get; set; }
    
}