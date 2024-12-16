using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TESTDB.DTO;
using TESTDB.Services.UserServices;

namespace TESTDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize] 
        public async Task<ActionResult<GetUserDto>> GetUserInfo()
        {
            var result = await _userService.GetUserInfo();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetUserDetails(int id)
        {
            var result = await _userService.GetUserDetails(id);
            return Ok(result);
        }

    }
}
