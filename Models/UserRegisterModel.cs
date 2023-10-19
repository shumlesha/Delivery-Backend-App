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
    
    
    [EmailAddress]
    public string email { get; set; }
    
    
    public string addressId { get; set;}
    
    
    public DateTime birthDate { get; set; }
    
    [Required]
    public string gender { get; set; }
    
    [Phone]
    public string phoneNumber { get; set; }
    
}