using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class DishInCart
{
    [Key]
    public Guid Id { get; set; }
    
    
    public string Name { get; set; }
    
    [Required]
    public int Count { get; set; }
    
    
    public Guid? OrderId { get; set; }
    public virtual Order Order { get; set; }
    
    
    public Guid DishId { get; set; }
    public virtual Dish Dish { get; set; }
    

    public Guid? UserId { get; set; }
    
    public virtual User User { get; set; }

}