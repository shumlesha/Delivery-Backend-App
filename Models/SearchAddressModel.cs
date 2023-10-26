namespace webNET_Hits_backend_aspnet_project_1.Models;

public class SearchAddressModel
{
    public long objectId { get; set; }
    
    public Guid objectGuid { get; set; }
    
    public string? text { get; set; }
    
    public GarAddressLevel objectLevel { get; set; }
    
    public string? objectLevelText { get; set; }
    
}