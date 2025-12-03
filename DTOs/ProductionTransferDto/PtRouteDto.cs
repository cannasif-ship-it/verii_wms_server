using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class PtRouteDto : BaseRouteEntityDto
    {
        public long ImportLineId { get; set; }
    }

    public class CreatePtRouteDto : BaseRouteCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
    }

    public class UpdatePtRouteDto : BaseRouteUpdateDto
    {
        public long? ImportLineId { get; set; }
    }
}