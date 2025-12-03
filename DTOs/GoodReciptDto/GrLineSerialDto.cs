using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class GrLineSerialDto : BaseLineSerialEntityDto
    {
        public long ImportLineId { get; set; }
    }

    public class CreateGrLineSerialDto : BaseLineSerialCreateDto
    {
        [Required]
        public long ImportLineId { get; set; }
    }

    public class UpdateGrLineSerialDto : BaseLineSerialUpdateDto
    {
        public long? ImportLineId { get; set; }
    }
}