using Microsoft.AspNetCore.Mvc;
using MPP_Server.model;
using MPP_Server.service;

namespace MPP_Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IService _service;

        public UsersController(IService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            var user = new User { Username = dto.Username, Password = dto.Password };
            // observer is not needed for HTTP, just pass null
            bool ok = _service.Login(user, null);
            if (ok)
                return Ok(new { username = user.Username });
            else
                return Unauthorized();
        }
    }
}