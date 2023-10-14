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
    
    
    public string Adress { get; set; }
    
    
    
    
}