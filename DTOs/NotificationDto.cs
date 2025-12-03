using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class NotificationDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public byte Channel { get; set; }
        public byte? Severity { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public long? RecipientUserId { get; set; }
        public long? RecipientTerminalUserId { get; set; }
        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }

    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "Notification_Title_Required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notification_Message_Required")]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notification_Channel_Required")]
        public byte Channel { get; set; }

        public byte? Severity { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }
        public long? RecipientTerminalUserId { get; set; }

        public List<long>? RecipientUserIds { get; set; }

        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }

    public class UpdateNotificationDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        public byte? Channel { get; set; }
        public byte? Severity { get; set; }

        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }
        public long? RecipientTerminalUserId { get; set; }

        public string? RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public string? ActionUrl { get; set; }
        public string? TerminalActionCode { get; set; }
    }
}
