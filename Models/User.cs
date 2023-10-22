using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace webNET_Hits_backend_aspnet_project_1.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    
    public string FullName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    [Required]
    [Phone]
    public string Phone { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    
    public string Address { get; set; }
    
    public string Password { get; set; }
    
    public List<Rating> Ratings { get; set; }
    public List<DishInCart> DishesInCart { get; set; }
    //public List<Dish> Dishes { get; set; }
    public List<Order> Orders { get; set; }
    
}