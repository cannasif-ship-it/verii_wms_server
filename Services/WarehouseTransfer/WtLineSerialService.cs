using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtLineSerialService : IWtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var entities = await _unitOfWork.WtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WtLineSerialDto>>(entities);
                return ApiResponse<IEnumerable<WtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> CreateAsync(CreateWtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 400);
                }
                var entity = _mapper.Map<WtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;

                await _unitOfWork.WtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtLineSerialDto>> UpdateAsync(long id, UpdateWtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }

                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineNotFound"), _localizationService.GetLocalizedString("WtLineNotFound"), 400);
                    }
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;

                _unitOfWork.WtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<WtLineSerialDto>(entity);
                return ApiResponse<WtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WtLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialNotFound"), _localizationService.GetLocalizedString("WtLineSerialNotFound"), 404);
                }
                await _unitOfWork.WtLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}