using Azure.Core;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IAddressService
{
    Task<List<SearchAddressModel>> SearchAddresses(long parentObjectId, string query);
}

public class AddressService : IAddressService
{
    private readonly GarDbContext _garContext;

    public AddressService(GarDbContext garContext)
    {
        _garContext = garContext;
    }

    public async Task<List<SearchAddressModel>> SearchAddresses(long parentObjectId, string query)
    {
        string[] allLevelsTexted =
        {
            "Субъект РФ",
            "Административный район",
            "Муниципальный район",
            "Сельское/городское поселение",
            "Город",
            "Населенный пункт",
            "Элемент планировочной структуры",
            "Элемент улично-дорожной сети",
            "Земельный участок",
            "Здание (сооружение)",
            "Помещение",
            "Помещение в пределах помещения",
            "Уровень автономного округа",
            "Уровень внутригородской территории",
            "Уровень дополнительных территорий", 
            "Уровень объектов на дополнительных территориях",
            "Машино-место"
        };
        
        
        var commonTable = _garContext.AsAddrObjs.Join(
            _garContext.AsAdmHierarchies,
            obj => obj.Objectid,
            hierarchy => hierarchy.Objectid,
            (obj, hierarchy) => new { obj, hierarchy }
        ).Where(common =>
            parentObjectId == common.hierarchy.Parentobjid &&
            common.obj.Name.Contains(query));

        return await commonTable.Select(line => new SearchAddressModel
        {
            objectId = line.obj.Objectid,
            objectGuid = line.obj.Objectguid,
            text = $"{line.obj.Typename} {line.obj.Name}",
            objectLevel = (GarAddressLevel)(int.Parse(line.obj.Level) - 1),
            objectLevelText = allLevelsTexted[int.Parse(line.obj.Level) - 1]
        }).ToListAsync();

    }
    
    
}