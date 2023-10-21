namespace webNET_Hits_backend_aspnet_project_1.Models.DTO;

public class DishPagedListDTO
{
    public List<DishDTO> Dishes { get; set; }
    public PageInfoModel Pagination { get; set; }
}