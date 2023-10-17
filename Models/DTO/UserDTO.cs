using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class UserDTO
{
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string Address { get; set; }
}