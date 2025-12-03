using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrImportDocumentService
    {
        Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrImportDocumentDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
        Task<ApiResponse<GrImportDocumentDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<GrImportDocumentDto>> CreateAsync(CreateGrImportDocumentDto createDto);
        Task<ApiResponse<GrImportDocumentDto>> UpdateAsync(long id, UpdateGrImportDocumentDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> ExistsAsync(long id);
    }
}