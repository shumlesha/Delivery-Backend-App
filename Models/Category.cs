using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace webNET_Hits_backend_aspnet_project_1.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum Category
{
    WOK,
    Pizza,
    Soup,
    Dessert,
    Drink
}