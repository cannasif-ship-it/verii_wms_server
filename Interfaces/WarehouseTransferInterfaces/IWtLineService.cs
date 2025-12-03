using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWtLineService
    {
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<WtLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<WtLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<WtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<WtLineDto>> CreateAsync(CreateWtLineDto createDto);
        Task<ApiResponse<WtLineDto>> UpdateAsync(long id, UpdateWtLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}