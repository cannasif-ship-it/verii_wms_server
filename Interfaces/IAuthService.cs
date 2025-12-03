using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> GetUserByIdAsync(long id);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterDto registerDto);
        Task<ApiResponse<string>> LoginAsync(LoginRequest loginDto);
    }
}