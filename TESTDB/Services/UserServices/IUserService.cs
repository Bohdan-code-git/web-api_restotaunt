using TESTDB.DTO;

namespace TESTDB.Services.UserServices
{
    public interface IUserService
    {
        Task<string> CreateUser(AddUserDto user);
        Task<string> Login(AddUserDto user);
        Task<GetUserDto> GetUserInfo();
        Task<GetUserDto?> GetUserDetails(int id);
        Task<string> ChangePassword(ChangePasswordDto user);

        //Task<string> ChangeEmail(string email);
        Task<string> ChangeName(string name);
        Task<string> ChangePhone(string phone);
        Task<string> UpdateUserCredentialsAsync(GetUserDto updateUser);

    }
}
