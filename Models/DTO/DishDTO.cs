using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishDTO
{

    [Required] public Guid id { get; set; }

    [Required]
    [MinLength(1)]
    public string name { get; set; }
    
    public string desctiption { get; set; }
    
    [Required]
    public double price { get; set; }
    
    public string image { get; set; }
    
    public bool vegeterian { get; set; }
    
    public double? rating { get; set; }
    
    public Category category  { get; set; }
    
}