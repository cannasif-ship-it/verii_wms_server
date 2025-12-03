using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WoLineSerialService : IWoLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WoLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.WoLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WoLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.WoLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WoLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineSerialDto>> CreateAsync(CreateWoLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WoLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 400);
                }
                var entity = _mapper.Map<WoLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.WoLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineSerialDto>> UpdateAsync(long id, UpdateWoLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WoLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WoLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.WoLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WoLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                await _unitOfWork.WoLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WoLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}