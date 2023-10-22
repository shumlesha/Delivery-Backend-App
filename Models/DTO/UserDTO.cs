using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class UserDTO
{
    [Required]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    
    [Required]
    public DateTime birthDate { get; set; }
    
    [Required]
    public string gender { get; set; }
    
    [Required]
    [Phone]
    public string phoneNumber { get; set; }
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    public string address { get; set; }
    
    
}