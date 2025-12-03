using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SitHeaderService : ISitHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SitHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(
                    dtos,
                    _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }

        public async Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.SitHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);

                bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy?.Trim())
                {
                    case "BranchCode":
                        query = desc ? query.OrderByDescending(x => x.BranchCode) : query.OrderBy(x => x.BranchCode);
                        break;
                    case "ERPReferenceNumber":
                        query = desc ? query.OrderByDescending(x => x.ERPReferenceNumber) : query.OrderBy(x => x.ERPReferenceNumber);
                        break;
                    case "CreatedDate":
                        query = desc ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate);
                        break;
                    default:
                        query = desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                        break;
                }

                var totalCount = await query.CountAsync();
                var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                var dtos = _mapper.Map<List<SitHeaderDto>>(items);
                var result = new PagedResponse<SitHeaderDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<SitHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
                    return ApiResponse<SitHeaderDto>.ErrorResult(nf, nf, 404);
                }
                var dto = _mapper.Map<SitHeaderDto>(entity);
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var entities = await _unitOfWork.SitHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                }
                if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.YearCode?.Length != 4)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.PlannedDate == default)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                var entity = _mapper.Map<SitHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SitHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitHeaderDto>(entity);
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SitHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitHeaderDto>(entity);
                return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitHeaders.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
                }
                await _unitOfWork.SitHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                _unitOfWork.SitHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
