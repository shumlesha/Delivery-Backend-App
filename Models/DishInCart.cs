﻿using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class DishInCart
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public int Count { get; set; }
}