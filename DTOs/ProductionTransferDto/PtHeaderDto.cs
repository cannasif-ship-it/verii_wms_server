using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtHeaderDto : BaseHeaderEntityDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }

    public class CreatePtHeaderDto : BaseHeaderCreateDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }

    public class UpdatePtHeaderDto : BaseHeaderUpdateDto
    {
        public string? CustomerCode { get; set; }
        public string? SourceWarehouse { get; set; }
        public string? TargetWarehouse { get; set; }
    }
}