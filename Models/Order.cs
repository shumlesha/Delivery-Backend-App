using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Order
{
    [Key]
    public Guid id { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public DateTime OrderTime { get; set; }
    
    public double Price { get; set; }
    
    public Guid AddressId { get; set; }
    
    public Status Status { get; set; }
}