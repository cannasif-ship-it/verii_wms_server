using AutoMapper;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PtLineSerialService : IPtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.PtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.PtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineSerialDto>> CreateAsync(CreatePtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.PtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 400);
                }
                var entity = _mapper.Map<PtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.PtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineSerialDto>> UpdateAsync(long id, UpdatePtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtLineSerials.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.PtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.PtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PtLineSerials.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                await _unitOfWork.PtLineSerials.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PtLineSerialDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}