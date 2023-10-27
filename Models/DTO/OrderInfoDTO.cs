using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class OrderInfoDTO
{
    public Guid id { get; set; }
    
    [Required]
    public DateTime deliveryTime { get; set; }
    
    [Required]
    public DateTime orderTime { get; set; }
    
    [Required]
    public Status status { get; set; }
    
    [Required]
    public double price { get; set; }
}