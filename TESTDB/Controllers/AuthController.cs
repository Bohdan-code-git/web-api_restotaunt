using Microsoft.AspNetCore.Mvc;
using TESTDB.DTO;
using TESTDB.Services.UserServices;

namespace TESTDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] AddUserDto newUser)
        {
            try
            {
                var result = await _userService.CreateUser(newUser);
                TokenDto tokenDto = new TokenDto();
                tokenDto.Token = result;
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(AddUserDto user)
        {
            try
            {
                var result = await _userService.Login(user);
                TokenDto tokenDto = new TokenDto();
                tokenDto.Token = result;
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
