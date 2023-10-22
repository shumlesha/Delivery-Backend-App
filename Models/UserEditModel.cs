using System;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_1.Models;

public class UserEditModel
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    
    [Required]
    public DateTime birthDate { get; set; }
    
    [Required]
    public string gender { get; set; }
    
    [Required]
    public string addressId { get; set; }
    
    [Phone]
    [Required]
    public string phoneNumber { get; set; }
}