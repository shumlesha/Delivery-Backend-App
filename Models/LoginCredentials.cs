using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class LoginCredentials
{
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string email { get; set; }
    
    [Required]
    [MinLength(1)]
    public string password { get; set; }
}