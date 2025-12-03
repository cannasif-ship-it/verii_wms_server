using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class WiTerminalLineDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
    }

    public class CreateWiTerminalLineDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long TerminalUserId { get; set; }
    }

    public class UpdateWiTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}