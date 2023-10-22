using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class LoginCredentials
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    public string password { get; set; }
}