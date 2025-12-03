using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtRouteDto : BaseRouteEntityDto
    {
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
    }

    public class CreateSrtRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSrtRouteDto : BaseRouteUpdateDto
    {
        public long? LineId { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public int? Priority { get; set; }
        public string? Description { get; set; }
    }
}
