using TESTDB.DTO;

namespace TESTDB.Services.UserServices
{
    public interface IUserService
    {
        Task<string> CreateUser(AddUserDto user);
        Task<string> Login(AddUserDto user);
       // Task<GetUserDto> GetUserInfo();
       //<GetUserDto?> GetUserDetails(int id);
    }
}
