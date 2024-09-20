using RestAPI_using_.Netcore8.Models;
using RestAPI_using_.Netcore8.Models.DTOs.UserDto;
namespace RestAPI_using_.Netcore8.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUser(string id);
        bool IsUniqueUser(string userName);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserLoginResponseDto> Register(UserRegistrationDto userRegistrationDto);
    }
}