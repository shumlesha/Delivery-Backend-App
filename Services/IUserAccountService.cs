using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IUserAccountService
{
    Task<string> UserRegister(UserDTO userDTO);
    Task<string> UserLogin(UserDTO userDTO);
}


public class UserAccountService: IUserAccountService
{
    private readonly AppDbContext _context;

    public UserAccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> UserRegister(UserDTO userDTO)
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> UserLogin(UserDTO userDTO)
    {
        throw new NotImplementedException();
    }
}