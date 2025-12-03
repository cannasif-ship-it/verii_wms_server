using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("admin-login")]
        public async Task<ActionResult<ApiResponse<string>>> AdminLogin()
        {
            var loginDto = new LoginRequest
            {
                Email = "admin@v3rii.com",
                Password = "Veriipass123!"
            };
            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
        {
            var result = await _authService.GetAllUsersAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(long id)
        {
            var result = await _authService.GetUserByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterUserAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }

    }
}