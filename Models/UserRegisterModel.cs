using System;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class UserRegisterModel
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    
    [Required]
    [MinLength(6)]
    public string password { get; set; }
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    
    public string addressId { get; set;}
    
    
    public DateTime birthDate { get; set; }
    
    [Required]
    public Gender gender { get; set; }
    
    [RegularExpression(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}", ErrorMessage="Wrong phone number!")]
    public string phoneNumber { get; set; }
    
}