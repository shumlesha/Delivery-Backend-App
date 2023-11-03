using System.Text;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_1.Models;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IAddressService
{
    Task<List<SearchAddressModel>> SearchAddresses(long parentObjectId, string query);

    Task<List<SearchAddressModel?>> GetAddressChain(Guid objectGuid);
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
        
        
        var commonObjTable = _garContext.AsAddrObjs.Join(
            _garContext.AsAdmHierarchies,
            obj => obj.Objectid,
            hierarchy => hierarchy.Objectid,
            (obj, hierarchy) => new { obj, hierarchy }
        ).Where(common =>
            parentObjectId == common.hierarchy.Parentobjid && common.obj.Isactual == 1 &&
            common.obj.Name.Contains(query));

        var allObjs = await commonObjTable.Select(line => new SearchAddressModel
        {
            objectId = line.obj.Objectid,
            objectGuid = line.obj.Objectguid,
            text = $"{line.obj.Typename} {line.obj.Name}",
            objectLevel = (GarAddressLevel)(int.Parse(line.obj.Level) - 1),
            objectLevelText = allLevelsTexted[int.Parse(line.obj.Level) - 1]
        }).ToListAsync();
        
        var commonHouseTable = await _garContext.AsAdmHierarchies.Join(
                _garContext.AsHouses,
                hierarchy => hierarchy.Objectid,
                house => house.Objectid,
                (hierarchy, house) => new { hierarchy, house }
            )
            .Where(common => parentObjectId == common.hierarchy.Parentobjid)
            .ToListAsync();

        var filteredHouseTable = commonHouseTable
            .Where(common =>
                GetBuildingText(common.house.Housetype, common.house.Housenum, common.house.Addtype1, common.house.Addnum1,
                    common.house.Addtype2, common.house.Addnum2).Contains(query) &&
                common.house.Isactual == 1
            );

        var allBuilds = filteredHouseTable.Select(line => new SearchAddressModel
        {
            objectId = line.house.Objectid,
            objectGuid = line.house.Objectguid,
            text = GetBuildingText(line.house.Housetype, line.house.Housenum, line.house.Addtype1, line.house.Addnum1,
                line.house.Addtype2, line.house.Addnum2),
            objectLevel = GarAddressLevel.Building,
            objectLevelText = "Здание (сооружение)"
        });

        return allObjs.Union(allBuilds).OrderBy(obj => obj.objectLevel).ToList();
    }


    public async Task<List<SearchAddressModel?>> GetAddressChain(Guid objectGuid)
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
        
        var ObjectID = await (from obj in _garContext.AsAddrObjs
                where obj.Objectguid == objectGuid
                select obj.Objectid)
            .Union(from house in _garContext.AsHouses
                where house.Objectguid == objectGuid
                select house.Objectid)
            .FirstOrDefaultAsync();

        var path =  _garContext.AsAdmHierarchies.Where(line => line.Objectid == ObjectID)
            .Select(line => line.Path).FirstOrDefault();

        var idsList = path.Split('.').Select(long.Parse).ToList();

        var chain = new List<SearchAddressModel?>();

        for (int i = 0; i < idsList.Count(); i++)
        {
            var id = idsList[i];
        
            var objectModel = await (from obj in _garContext.AsAddrObjs
                where obj.Objectid == id && obj.Isactive == 1
                select obj).FirstOrDefaultAsync();
                                 
            var houseModel = await (from house in _garContext.AsHouses
                where house.Objectid == id && house.Isactive == 1
                select house).FirstOrDefaultAsync();
        
            if (objectModel != null)
            {
                chain.Add(new SearchAddressModel
                {
                    objectId = objectModel.Objectid,
                    objectGuid = objectModel.Objectguid,
                    text = $"{objectModel.Typename} {objectModel.Name}",
                    objectLevel = (GarAddressLevel)(int.Parse(objectModel.Level) - 1),
                    objectLevelText = allLevelsTexted[int.Parse(objectModel.Level) - 1]
                });
            }
            else if (houseModel != null)
            {
                chain.Add(new SearchAddressModel
                {
                    objectId = houseModel.Objectid,
                    objectGuid = houseModel.Objectguid,
                    text = GetBuildingText(houseModel.Housetype, houseModel.Housenum, houseModel.Addtype1, houseModel.Addnum1,
                        houseModel.Addtype2, houseModel.Addnum2),
                    objectLevel = GarAddressLevel.Building,
                    objectLevelText = "Здание (сооружение)"
                });
            }
        }

        return chain;

    }

    public string GetBuildingText(int? houseType, string houseNum, int? addType1, string addNum1, int? addType2,
        string addNum2)
    {
        var houseTypes = new Dictionary<int, string>
        {
            {1, "влд."},
            {2, "д."},
            {3, "двлд."},
            {4, "г-ж."},
            {5, "зд."},
            {6, "Шахта"},
            {7, "Строение"},
            {8, "Сооружение"},
            {9, "Литера"},
            {10, "Корпус"},
            {11, "Подвал"},
            {12, "Котельная"},
            {13, "Погреб"},
            {14, "Объект незавершенного строительства (ОНС)"}
        };
        
        var addTypes = new Dictionary<int, string>
        {
            {1, "к."},
            {2, "стр."},
            {3, "соор."},
            {4, "литера"}
        };
        
        var fullBuildText = new StringBuilder();

        if (houseType.HasValue)
        {
            fullBuildText.Append(houseTypes[houseType.Value]);
            fullBuildText.Append(" " + houseNum);
        }
        
        if (addType1.HasValue && !string.IsNullOrEmpty(addNum1))
        {
            fullBuildText.Append(" " + (addTypes[addType1.Value]));
            fullBuildText.Append(" " + addNum1);
        }

        if (addType2.HasValue && !string.IsNullOrEmpty(addNum2))
        {
            fullBuildText.Append(" " + (addTypes[addType2.Value]));
            fullBuildText.Append(" " + addNum2);
        }

        return fullBuildText.ToString().Trim();
    }
    
    
}