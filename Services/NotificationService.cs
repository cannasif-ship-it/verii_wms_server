using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using WMS_WEBAPI.Hubs;

namespace WMS_WEBAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHubContext<NotificationHub> notificationHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _notificationHub = notificationHub;
        }

        public async Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto)
        {
            try
            {
                var entity = _mapper.Map<Notification>(dto);

                await _unitOfWork.Notifications.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<NotificationDto>(entity);

                await PublishSignalRNotificationAsync(entity);
                return ApiResponse<NotificationDto>.SuccessResult(result, _localizationService.GetLocalizedString("NotificationCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<NotificationDto>.ErrorResult(_localizationService.GetLocalizedString("NotificationCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<NotificationDto>>> CreateForUsersAsync(CreateNotificationDto dto)
        {
            try
            {
                using var tx = await _unitOfWork.BeginTransactionAsync();
                var recipients = dto.RecipientUserIds ?? new List<long>();
                if (dto.RecipientUserId.HasValue)
                {
                    recipients.Add(dto.RecipientUserId.Value);
                }

                if (recipients.Count == 0)
                {
                    return ApiResponse<IEnumerable<NotificationDto>>.ErrorResult(_localizationService.GetLocalizedString("NotificationRecipientsRequired"), _localizationService.GetLocalizedString("NotificationRecipientsRequired"), 400);
                }

                try
                {
                    var entities = new List<Notification>();
                    foreach (var userId in recipients.Distinct())
                    {
                        var entity = _mapper.Map<Notification>(dto);
                        entity.RecipientUserId = userId;
                        entities.Add(entity);
                    }

                    await _unitOfWork.Notifications.AddRangeAsync(entities);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.CommitTransactionAsync();

                    foreach (var entity in entities)
                    {
                        await PublishSignalRNotificationAsync(entity);
                    }

                    var dtos = _mapper.Map<List<NotificationDto>>(entities);
                    return ApiResponse<IEnumerable<NotificationDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("NotificationBulkCreatedSuccessfully"));
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<NotificationDto>>.ErrorResult(_localizationService.GetLocalizedString("NotificationBulkCreationError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<NotificationDto>>> GetByRecipientUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.Notifications
                    .FindAsync(x => x.RecipientUserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<List<NotificationDto>>(entities);
                return ApiResponse<IEnumerable<NotificationDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("NotificationRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<NotificationDto>>.ErrorResult(_localizationService.GetLocalizedString("NotificationRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> MarkAsReadAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.Notifications.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("NotificationNotFound"), _localizationService.GetLocalizedString("NotificationNotFound"), 404);
                }

                entity.IsRead = true;
                entity.ReadDate = DateTime.UtcNow;
                _unitOfWork.Notifications.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NotificationMarkedReadSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("NotificationMarkReadError"), ex.Message, 500);
            }
        }

        private async Task PublishSignalRNotificationAsync(Notification entity)
        {
            var type = entity.Severity switch
            {
                1 => "info",
                2 => "warning",
                3 => "error",
                _ => "info"
            };

            var payload = new
            {
                id = entity.Id,
                title = entity.Title,
                message = entity.Message,
                type,
                timestamp = DateTime.UtcNow,
                channel = entity.Channel,
                recipientUserId = entity.RecipientUserId,
                recipientTerminalUserId = entity.RecipientTerminalUserId,
            };

            if (entity.RecipientUserId.HasValue)
            {
                await NotificationHub.SendNotificationToUser(_notificationHub, entity.RecipientUserId.Value.ToString(), payload);
            }

            if (entity.RecipientTerminalUserId.HasValue)
            {
                await NotificationHub.SendNotificationToUser(_notificationHub, entity.RecipientTerminalUserId.Value.ToString(), payload);
            }

            if (!entity.RecipientUserId.HasValue && !entity.RecipientTerminalUserId.HasValue)
            {
                await NotificationHub.SendNotificationToAll(_notificationHub, payload);
            }
        }
    }
}
