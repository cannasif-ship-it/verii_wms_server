using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitLineSerialService : ISitLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SitLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.SitLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.SitLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SitLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> CreateAsync(CreateSitLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.SitLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
                }
                var entity = _mapper.Map<SitLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.SitLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitLineSerialDto>> UpdateAsync(long id, UpdateSitLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.SitLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineNotFound"), _localizationService.GetLocalizedString("SitLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.SitLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitLineSerialDto>(entity);
                return ApiResponse<SitLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialNotFound"), _localizationService.GetLocalizedString("SitLineSerialNotFound"), 404);
                }
                await _unitOfWork.SitLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}