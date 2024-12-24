using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            // Найти пользователя по email
            var user = postgreSqlContext.Users.FirstOrDefault(u => u.Email == changePasswordDto.Email);
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (user == null)
                throw new Exception("User not found");

            //if (user.Email == result)
            //    throw new Exception("User is not yours");

            // Проверить текущий пароль
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.Password))
                throw new Exception("Current password is incorrect");

            // Хэшировать новый пароль
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            // Обновить пароль пользователя
            user.Password = newPasswordHash;
            postgreSqlContext.Users.Update(user);
            await postgreSqlContext.SaveChangesAsync();

            // Сгенерировать новый токен
            string token = CreateToken(user);

            return token;
        }

        //public async Task<string> ChangeEmail(string email)
        //{
        //    // Получить текущего пользователя (например, через токен или другой метод аутентификации)
        //    var user = GetUserInfo();
        //    if (user == null)
        //        throw new Exception("User not found");

        //    // Проверить, не используется ли email другим пользователем
        //    var existingUser = postgreSqlContext.Users.FirstOrDefault(u => u.Email == email);
        //    if (existingUser != null)
        //        throw new Exception("Email is already in use");


        //    User newuser = new();
        //    newuser.Email = email;
        //    newuser.Password=
        //    existingUser.Email = email;
        //    postgreSqlContext.Users.Update(existingUser);
        //    await postgreSqlContext.SaveChangesAsync();

        //    // Сгенерировать новый токен
        //    string token = CreateToken(existingUser);

        //    return token;
        //}

        public async Task<string> ChangeName(string name)
        {
            // Получить текущего пользователя
            var user = GetUserInfo();
            if (user == null)
                throw new Exception("User not found");
            var existingUser = postgreSqlContext.Users.FirstOrDefault(u => u.Email == user.Result.Email);
            if (existingUser == null)
                throw new Exception("Email is already in use");

            // Обновить имя
            existingUser.UserName = name;
            postgreSqlContext.Users.Update(existingUser);
            await postgreSqlContext.SaveChangesAsync();

            // Сгенерировать новый токен
            string token = CreateToken(existingUser);

            return token;
        }

        public async Task<string> ChangePhone(string phone)
        {
            // Получить текущего пользователя
            var user = GetUserInfo();
            if (user == null)
                throw new Exception("User not found");

            var existingUser = postgreSqlContext.Users.FirstOrDefault(u => u.Email == user.Result.Email);
            if (existingUser == null)
                throw new Exception("Email is already in use");

            if (!Regex.IsMatch(phone, @"^\+?[1-9]\d{1,14}$"))
                throw new Exception("Invalid phone number format");

            // Обновить номер телефона
            existingUser.PhoneNumber = phone;
            postgreSqlContext.Users.Update(existingUser);
            await postgreSqlContext.SaveChangesAsync();

            // Сгенерировать новый токен
            string token = CreateToken(existingUser);

            return token;
        }

    }
}
