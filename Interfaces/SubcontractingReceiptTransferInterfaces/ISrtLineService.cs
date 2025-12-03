using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtLineService
    {
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto);
        Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}