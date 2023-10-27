using Microsoft.AspNetCore.Mvc;
using webNET_Hits_backend_aspnet_project_1.Services;

namespace webNET_Hits_backend_aspnet_project_1.Controllers;


[ApiController]
[Route("api/address")]
public class AddressController: ControllerBase
{
    private IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAddresses(long parentObjectId, string query="")
    {
        return Ok(await _addressService.SearchAddresses(parentObjectId, query));
    }


    [HttpGet("getaddresschain")]
    public async Task<IActionResult> GetAddressChain(Guid objectGuid)
    {
        return Ok(await _addressService.GetAddressChain(objectGuid));
    }
}