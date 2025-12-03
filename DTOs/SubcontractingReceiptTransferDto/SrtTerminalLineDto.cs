using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class SrtTerminalLineDto : BaseEntityDto
    {
        public long HeaderId { get; set; }
        public long TerminalUserId { get; set; }
    }

    public class CreateSrtTerminalLineDto
    {
        [Required]
        public long HeaderId { get; set; }

        [Required]
        public long TerminalUserId { get; set; }
    }

    public class UpdateSrtTerminalLineDto
    {
        public long? HeaderId { get; set; }
        public long? TerminalUserId { get; set; }
    }
}