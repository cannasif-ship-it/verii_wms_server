using System;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISidebarmenuHeaderService
    {
        Task<ApiResponse<IEnumerable<SidebarmenuHeaderDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SidebarmenuHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<SidebarmenuHeaderDto>> GetByIdAsync(long id);
        Task<ApiResponse<SidebarmenuHeaderDto>> CreateAsync(CreateSidebarmenuHeaderDto createDto);
        Task<ApiResponse<SidebarmenuHeaderDto>> UpdateAsync(long id, UpdateSidebarmenuHeaderDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> ExistsAsync(long id);
        Task<ApiResponse<SidebarmenuHeaderDto>> GetByMenuKeyAsync(string menuKey);
        Task<ApiResponse<IEnumerable<SidebarmenuHeaderDto>>> GetByRoleLevelAsync(int roleLevel);
        Task<ApiResponse<List<SidebarmenuHeader>>> GetSidebarmenuHeadersByUserId(int userId);
        
    }
}