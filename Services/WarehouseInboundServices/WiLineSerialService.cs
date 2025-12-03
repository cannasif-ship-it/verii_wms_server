using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiLineSerialService : IWiLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WiLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WiLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                }
                var entity = _mapper.Map<WiLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.WiLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WiLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.WiLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.WiLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                await _unitOfWork.WiLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}