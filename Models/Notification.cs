using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_NOTIFICATION")]
    public class Notification : BaseEntity
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Message { get; set; } = string.Empty;

        public byte Channel { get; set; }

        public byte? Severity { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; }

        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }
        [ForeignKey(nameof(RecipientUserId))]
        public virtual User? RecipientUser { get; set; }

        public long? RecipientTerminalUserId { get; set; }
        [ForeignKey(nameof(RecipientTerminalUserId))]
        public virtual User? RecipientTerminalUser { get; set; }

        [MaxLength(100)]
        public string? RelatedEntityType { get; set; }

        public long? RelatedEntityId { get; set; }

        [MaxLength(250)]
        public string? ActionUrl { get; set; }

        [MaxLength(50)]
        public string? TerminalActionCode { get; set; }
    }
}
