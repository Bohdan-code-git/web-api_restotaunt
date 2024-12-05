using TESTDB.DTO;

namespace TESTDB.Services.UserServices
{
    public interface IUserService
    {
        Task<string> CreateUser(AddUserDto user);
        Task<string> Login(AddUserDto user);
        Task<GetUserDto> GetUserInfo();
        Task<GetUserDto?> GetUserDetails(int id);
    }
}
