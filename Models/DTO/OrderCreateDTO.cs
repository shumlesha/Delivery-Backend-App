using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class OrderCreateDTO
{
    [Required]
    public DateTime deliveryTime { get; set; }
    
    [Required]
    public Guid addressId { get; set; }
    
}