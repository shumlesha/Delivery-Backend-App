using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class Hierarchy
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ObjectId { get; set; }
    
    public Guid? ParentObjectId { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    public List <House> Houses { get; set; }
    
    public AddressElement Parent { get; set; }
    
}