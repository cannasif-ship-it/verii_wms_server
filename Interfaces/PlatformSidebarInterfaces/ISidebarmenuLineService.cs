using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISidebarmenuLineService
    {
        Task<ApiResponse<IEnumerable<SidebarmenuLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SidebarmenuLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<SidebarmenuLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<SidebarmenuLineDto>> CreateAsync(CreateSidebarmenuLineDto createDto);
        Task<ApiResponse<SidebarmenuLineDto>> UpdateAsync(long id, UpdateSidebarmenuLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> ExistsAsync(long id);
        Task<ApiResponse<IEnumerable<SidebarmenuLineDto>>> GetByHeaderIdAsync(int headerId);
        Task<ApiResponse<SidebarmenuLineDto>> GetByPageAsync(string page);
    }
}