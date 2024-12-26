using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TESTDB.DATA;
using TESTDB.DTO;
using TESTDB.Models;


namespace TESTDB.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly PostgreSqlContext postgreSqlContext;
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(
            PostgreSqlContext postgreSqlContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.postgreSqlContext = postgreSqlContext;
            this.mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Login(AddUserDto user)
        {
            var result = postgreSqlContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (result == null)
                throw new Exception("User's not found");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, result.Password))
                throw new Exception("Wrong credentials");

            string token = CreateToken(result);

            return token;
        }
        public async Task<string> CreateUser(AddUserDto newUser)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = passwordHash;

            User user = mapper.Map<User>(newUser);

            var result = postgreSqlContext.Users.FirstOrDefault(u => u.Email == newUser.Email);

            if (result !=null)
                throw new Exception("user already exist");

            postgreSqlContext.Users.Add(user);
            await postgreSqlContext.SaveChangesAsync();

            string token = CreateToken(mapper.Map<User>(newUser));

            return token;
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(100),
                    signingCredentials: creds,
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"]
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<GetUserDto> GetUserInfo()
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                
                var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

                var user = postgreSqlContext.Users.FirstOrDefault(u => u.Email == result);
                
                if (user == null)
                    return null;

                return mapper.Map<GetUserDto>(user);
            }
            return null;
        }

        public async Task<GetUserDto?> GetUserDetails(int id)
        {
            var user = postgreSqlContext.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
                return null;

            return mapper.Map<GetUserDto>(user);
        }
        public async Task<string> UpdateUserCredentialsAsync(GetUserDto updateUser)
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return "Bad request: HttpContext is null.";
            }

            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return "Bad request: User email not found in claims.";
            }

            var user = await postgreSqlContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null)
            {
                return "Bad request: User not found.";
            }

            user.Adress = !string.IsNullOrEmpty(updateUser.Adress) ? updateUser.Adress : user.Adress;

            user.UserName = !string.IsNullOrEmpty(updateUser.UserName) ? updateUser.UserName : user.UserName;

            user.PhoneNumber = !string.IsNullOrEmpty(updateUser.PhoneNumber) ? updateUser.PhoneNumber : user.PhoneNumber;

            user.Email = !string.IsNullOrEmpty(updateUser.Email) ? updateUser.Email : user.Email;

            postgreSqlContext.Users.Update(user);
            await postgreSqlContext.SaveChangesAsync();

            string token = CreateToken(user);

            return token;
        }
        public async Task<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            // Найти пользователя по email
            var user = postgreSqlContext.Users.FirstOrDefault(u => u.Email == changePasswordDto.Email);
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (user == null)
                throw new Exception("User not found");

            //if (user.Email != result)
            //    throw new Exception("User is not yours"); сложную логику проработать

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.Password))
                throw new Exception("Current password is incorrect");

            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            user.Password = newPasswordHash;
            postgreSqlContext.Users.Update(user);
            await postgreSqlContext.SaveChangesAsync();

            string token = CreateToken(user);

            return token;
        }

        public async Task<string> ChangeName(string name)
        {
            var user = GetUserInfo();
            if (user == null)
                throw new Exception("User not found");
            var existingUser = postgreSqlContext.Users.FirstOrDefault(u => u.Email == user.Result.Email);
            if (existingUser == null)
                throw new Exception("Email is already in use");

            existingUser.UserName = name;
            postgreSqlContext.Users.Update(existingUser);
            await postgreSqlContext.SaveChangesAsync();

            string token = CreateToken(existingUser);

            return token;
        }

        public async Task<string> ChangePhone(string phone)
        {
            var user = GetUserInfo();
            if (user == null)
                throw new Exception("User not found");

            var existingUser = postgreSqlContext.Users.FirstOrDefault(u => u.Email == user.Result.Email);
            if (existingUser == null)
                throw new Exception("Email is already in use");

            if (!Regex.IsMatch(phone, @"^\+?[1-9]\d{1,14}$"))
                throw new Exception("Invalid phone number format");


            existingUser.PhoneNumber = phone;
            postgreSqlContext.Users.Update(existingUser);
            await postgreSqlContext.SaveChangesAsync();

            string token = CreateToken(existingUser);

            return token;
        }

    }
}
