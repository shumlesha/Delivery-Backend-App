using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webNET_Hits_backend_aspnet_project_1.Models;
using webNET_Hits_backend_aspnet_project_1.Models.DTO;

namespace webNET_Hits_backend_aspnet_project_1.Services;

public interface IUserAccountService
{
    Task<string> UserRegister(UserRegisterModel userRegisterModel);
    Task<string> UserLogin(LoginCredentials loginCredentials);
    string GetJwtToken(User user);
    string PasswordToCrypto(string password);
    bool CheckPass(string possiblePassword, string existingPassword);

    Task<UserDTO> UserGetProfile(Guid userID);

    Task UserEditProfile(UserEditModel userEditModel, Guid guid);

    Task UserLogoutProfile(string token);
}


public class UserAccountService: IUserAccountService
{
    private readonly AppDbContext _context;
    private readonly JwtParams _JwtParams;
    public UserAccountService(AppDbContext context, IOptions<JwtParams> jwtParamsOptions)
    {
        _context = context;
        _JwtParams = jwtParamsOptions.Value;
    }

    public async Task<string> UserRegister(UserRegisterModel userRegisterModel)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = userRegisterModel.fullName,
            BirthDate = userRegisterModel.birthDate,
            Gender = userRegisterModel.gender,
            Phone = userRegisterModel.phoneNumber,
            Email = userRegisterModel.email,
            Address = userRegisterModel.addressId,
            Password = PasswordToCrypto(userRegisterModel.password)
        };
        var possibleUser = _context.Users.SingleOrDefault(u => u.Email == userRegisterModel.email);

        if (possibleUser != null)
        {
            throw new Exception("User already exists");
        }
        
        
        _context.Users.Add(user);

        await _context.SaveChangesAsync();


        return GetJwtToken(user);
    }
    
    public async Task<string> UserLogin(LoginCredentials loginCredentials)
    {
        var user = _context.Users.SingleOrDefault(user => user.Email == loginCredentials.email);

        if (user == null)
        {
            throw new AuthenticationException();
        }
        else if (!CheckPass(loginCredentials.password, user.Password))
        {
            throw new AuthenticationException(); 
        }

        return GetJwtToken(user);
    }


    public async Task UserLogoutProfile(string token)
    {
        _context.BannedTokens.Add(new BannedToken
        {
            Id = Guid.NewGuid(),
            TokenString = token,
            AdditionDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
    
    
    public async Task<UserDTO> UserGetProfile(Guid userID)
    {
        var user = await _context.Users.FindAsync(userID);

        return new UserDTO
        {
            id = user.Id,
            fullName = user.FullName,
            birthDate = user.BirthDate,
            gender = user.Gender,
            address = user.Address,
            email = user.Email,
            phoneNumber = user.Phone
        };
    }

    public async Task UserEditProfile(UserEditModel userEditModel, Guid guid)
    {
        var user = await _context.Users.FindAsync(guid);

        user.FullName = userEditModel.fullName;
        user.BirthDate = userEditModel.birthDate;
        user.Gender = userEditModel.gender;
        user.Address = userEditModel.addressId;
        user.Phone = userEditModel.phoneNumber;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

    }
    
    
    
    public string GetJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)

        };
       
        var token = new JwtSecurityToken(
            issuer: _JwtParams.Issuer,
            audience: _JwtParams.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1), 
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_JwtParams.Key)), SecurityAlgorithms.HmacSha256)

        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        string convertedToken = tokenHandler.WriteToken(token);
        
        return convertedToken;
    }
    
    public string PasswordToCrypto(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] entryData = Encoding.ASCII.GetBytes(password);
            byte[] hashvalue = sha256.ComputeHash(entryData);

            return BitConverter.ToString(hashvalue).Replace("-", "").ToLower();
        }
        
    }
    
    public bool CheckPass(string possiblePassword, string existingPassword)
    {
        string possibleHashed = PasswordToCrypto(possiblePassword);
        return possibleHashed == existingPassword;

    }
    
}