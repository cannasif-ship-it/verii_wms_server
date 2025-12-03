using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtImportLineDto
    {
        public long Id { get; set; }
        public long HeaderId { get; set; }
        public long LineId { get; set; }
        public long? RouteId { get; set; }
        public string StockCode { get; set; } = string.Empty;
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? CreatedByFullUser { get; set; }
        public string? UpdatedByFullUser { get; set; }
        public string? DeletedByFullUser { get; set; }
    }

    public class CreateSrtImportLineDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long LineId { get; set; }

        public long? RouteId { get; set; }

        [Required]
        [StringLength(35)]
        public string StockCode { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Description1 { get; set; }

        [StringLength(50)]
        public string? Description2 { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }

    public class UpdateSrtImportLineDto
    {
        public long? HeaderId { get; set; }
        public long? LineId { get; set; }
        public long? RouteId { get; set; }

        [StringLength(35)]
        public string? StockCode { get; set; }

        [StringLength(30)]
        public string? Description1 { get; set; }

        [StringLength(50)]
        public string? Description2 { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}