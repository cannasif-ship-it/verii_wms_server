using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IWiRouteService
    {
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetAllAsync();
        Task<ApiResponse<WiRouteDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetByStockCodeAsync(string stockCode);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetBySerialNoAsync(string serialNo);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse);
        Task<ApiResponse<IEnumerable<WiRouteDto>>> GetByQuantityRangeAsync(decimal minQuantity, decimal maxQuantity);
        Task<ApiResponse<WiRouteDto>> CreateAsync(CreateWiRouteDto createDto);
        Task<ApiResponse<WiRouteDto>> UpdateAsync(long id, UpdateWiRouteDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}