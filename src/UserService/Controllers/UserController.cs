using UserService.Model;
using UserService.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("user-api")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.IsSuccess);
        }

        [HttpPatch("user/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.IsSuccess);
        }

        [HttpGet]
        [Route("user/{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            var user = _userService.GetUserByIdAsync(id);
            if (user == null) {
                return BadRequest("Invalid user");
            }
            return Ok(user);
        }
    }
}
