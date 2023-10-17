using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class House
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ObjectId { get; set; }
    
    [Required]
    public Guid ObjectGuid { get; set; }
    
    [Required]
    public string HouseNum { get; set; }
    
    public string AddNum1 { get; set; }
    public string AddNum2 { get; set; }
    
    [Required]
    public string AddType1 { get; set; }
    
    [Required]
    public string AddType2 { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    public Guid HierarchyId { get; set; }
    public Hierarchy Hierarchy { get; set; }
}