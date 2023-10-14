using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Rating
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public double Value { get; set; }
    
    
}