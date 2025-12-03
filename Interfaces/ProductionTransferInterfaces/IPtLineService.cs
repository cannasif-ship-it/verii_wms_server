using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtLineService
    {
        Task<ApiResponse<IEnumerable<PtLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PtLineDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<PtLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<PtLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<PtLineDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<PtLineDto>>> GetByErpOrderNoAsync(string erpOrderNo);
        Task<ApiResponse<IEnumerable<PtLineDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<PtLineDto>> CreateAsync(CreatePtLineDto createDto);
        Task<ApiResponse<PtLineDto>> UpdateAsync(long id, UpdatePtLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}