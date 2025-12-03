using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrImportDocumentController : ControllerBase
    {
        private readonly IGrImportDocumentService _grImportDocumentService;
        private readonly ILocalizationService _localizationService;

        public GrImportDocumentController(IGrImportDocumentService grImportDocumentService, ILocalizationService localizationService)
        {
            _grImportDocumentService = grImportDocumentService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportDocumentDto>>>> GetAll()
        {
            var result = await _grImportDocumentService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> GetById(long id)
        {
            var result = await _grImportDocumentService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportDocumentDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _grImportDocumentService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> Create([FromBody] CreateGrImportDocumentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grImportDocumentService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportDocumentDto>>> Update(long id, [FromBody] UpdateGrImportDocumentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrImportDocumentDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grImportDocumentService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _grImportDocumentService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// Sayfalı GrImportDocument kayıtlarını getirir
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="sortBy">Sıralama alanı (Id, HeaderId, CreatedDate)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalı GrImportDocument listesi</returns>
        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrImportDocumentDto>>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc")
        {
            var result = await _grImportDocumentService.GetPagedAsync(pageNumber, pageSize, sortBy, sortDirection);
            return StatusCode(result.StatusCode, result);
        }
    }
}