using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatiliDost.Data.DTOs;
using PatiliDost.Services;

namespace PatiliDost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO model)
        {
            var result = await _userService.RegisterAsync(model);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<ActionResult> AddToRole(AddRoleDTO model)
        {
            var result = await _userService.AddRoleAsync(model);

            return Ok(result);
        }

        [HttpPost("gettoken")]
        public async Task<ActionResult> GetToken(TokenRequestDTO model)
        {
            var result = await _userService.GetTokenAsync(model);

            return Ok(result);
        }
    }
}
