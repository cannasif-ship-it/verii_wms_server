using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtLineSerialService : ISrtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SrtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.SrtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.SrtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> CreateAsync(CreateSrtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.SrtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 400);
                }
                var entity = _mapper.Map<SrtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.SrtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtLineSerialDto>> UpdateAsync(long id, UpdateSrtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.SrtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineNotFound"), _localizationService.GetLocalizedString("SrtLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.SrtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtLineSerialDto>(entity);
                return ApiResponse<SrtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SrtLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialNotFound"), _localizationService.GetLocalizedString("SrtLineSerialNotFound"), 404);
                }
                await _unitOfWork.SrtLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}