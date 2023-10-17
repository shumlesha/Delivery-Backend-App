using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class AddressElement
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ObjectId { get; set; }
    
    [Required]
    public Guid ObjectGuid { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string TypeName { get; set; }
    
    [Required]
    public int Level { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    
}