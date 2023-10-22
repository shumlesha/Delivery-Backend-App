using System;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class BannedToken
{
    public Guid Id { get; set; }
    
    public Guid UserID { get; set; }
    public string TokenString  { get; set; }
    
    public DateTime AdditionDate { get; set; }
    
}