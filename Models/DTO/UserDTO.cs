using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class UserDTO
{
    [Required]
    public string fullName { get; set; }
    
    [Required]
    public DateTime birthDate { get; set; }
    
    [Required]
    public string gender { get; set; }
    
    [Phone]
    public string phoneNumber { get; set; }
    
    [EmailAddress]
    public string email { get; set; }
    
    public string adressId { get; set; }
    
    public string password { get; set; }
}