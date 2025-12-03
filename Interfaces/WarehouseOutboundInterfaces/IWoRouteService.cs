using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWoRouteService
    {
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetAllAsync();
        Task<ApiResponse<WoRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse);
        Task<ApiResponse<IEnumerable<WoRouteDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<WoRouteDto>> CreateAsync(CreateWoRouteDto createDto);
        Task<ApiResponse<WoRouteDto>> UpdateAsync(long id, UpdateWoRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}