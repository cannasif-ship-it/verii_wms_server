using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto);
        Task<ApiResponse<IEnumerable<NotificationDto>>> CreateForUsersAsync(CreateNotificationDto dto);
        Task<ApiResponse<IEnumerable<NotificationDto>>> GetByRecipientUserIdAsync(long userId);
        Task<ApiResponse<bool>> MarkAsReadAsync(long id);
    }
}
