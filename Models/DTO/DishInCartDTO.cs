using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishInCartDTO
{
    [Required]
    public Guid DishId { get; set; }
    
    [Required]
    public int Count { get; set; }
    
    
}