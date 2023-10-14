using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class OrderDTO
{
    public DateTime DeliveryTime { get; set; }
    
    [Required]
    public DateTime OrderTime { get; set; }
    
    public Status Status { get; set; }
    
    public double Price { get; set; }
    
    public List<DishInCartDTO> Dishes  { get; set; }
    
}