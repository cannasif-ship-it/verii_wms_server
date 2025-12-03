using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtHeaderDto : BaseHeaderEntityDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
        public byte Type { get; set; }
    }

    public class CreateSrtHeaderDto : BaseHeaderCreateDto
    {
        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        [Required]
        public byte Type { get; set; }
    }

    public class UpdateSrtHeaderDto : BaseHeaderUpdateDto
    {
        [StringLength(20)]
        public string? CustomerCode { get; set; }

        [StringLength(20)]
        public string? SourceWarehouse { get; set; }

        [StringLength(20)]
        public string? TargetWarehouse { get; set; }

        public byte? Type { get; set; }
    }
}