using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class DishInCart
{
    [Key]
    public Guid Id { get; set; }
    
    
    [Required]
    public int Count { get; set; }
    
    
    public Guid? OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
    
    
    public Guid DishId { get; set; }
    
    [ForeignKey("DishId")]
    public virtual Dish Dish { get; set; }
    

    public Guid? UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

}