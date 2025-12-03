using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WtRouteDto : BaseRouteEntityDto
    {
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public string? Description { get; set; }

    }

    public class CreateWtRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long LineId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? YapKod { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateWtRouteDto : BaseRouteUpdateDto
    {
        public long? LineId { get; set; }
        public string? StockCode { get; set; }
        public string? YapKod { get; set; }
        public string? Description { get; set; }
    }
}
