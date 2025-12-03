using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GrLineController : ControllerBase
    {
        private readonly IGrLineService _grLineService;
        private readonly ILocalizationService _localizationService;

        public GrLineController(IGrLineService grLineService, ILocalizationService localizationService)
        {
            _grLineService = grLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm GR Line kayıtlarını getirir
        /// </summary>
        /// <returns>GR Line listesi</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrLineDto>>>> GetAll()
        {
            var result = await _grLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre GR Line kaydını getirir
        /// </summary>
        /// <param name="id">GR Line ID</param>
        /// <returns>GR Line detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> GetById(long id)
        {
            var result = await _grLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre GR Line kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">GR Header ID</param>
        /// <returns>GR Line listesi</returns>
        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrLineDto>>>> GetByHeaderId(long headerId)
        {
            var result = await _grLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni GR Line kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak GR Line bilgileri</param>
        /// <returns>Oluşturulan GR Line</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> Create([FromBody] CreateGrLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut GR Line kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek GR Line ID</param>
        /// <param name="updateDto">Güncellenecek GR Line bilgileri</param>
        /// <returns>Güncellenmiş GR Line</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> Update(long id, [FromBody] UpdateGrLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GR Line kaydını soft delete yapar
        /// </summary>
        /// <param name="id">Silinecek GR Line ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _grLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// Sayfalı GrLine kayıtlarını getirir
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="sortBy">Sıralama alanı (Id, HeaderId, Quantity, CreatedDate)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <returns>Sayfalı GrLine listesi</returns>
        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrLineDto>>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc")
        {
            var result = await _grLineService.GetPagedAsync(pageNumber, pageSize, sortBy, sortDirection);
            return StatusCode(result.StatusCode, result);
        }
    }
}