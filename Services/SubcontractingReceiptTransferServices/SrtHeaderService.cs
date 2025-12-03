using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class SrtHeaderService : ISrtHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SrtHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync()
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var query = _unitOfWork.SrtHeaders.AsQueryable().Where(x => !x.IsDeleted && x.BranchCode == branchCode);

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
                var dtos = _mapper.Map<List<SrtHeaderDto>>(items);
                var result = new PagedResponse<SrtHeaderDto>(dtos, totalCount, pageNumber, pageSize);
                return ApiResponse<PagedResponse<SrtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var notFound = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(notFound, notFound, 404);
                }
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByBranchCodeAsync(string branchCode)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.BranchCode == branchCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByCustomerCodeAsync(string customerCode)
        {
            try
            {
                var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.CustomerCode == customerCode && !x.IsDeleted && x.BranchCode == branchCode);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentTypeAsync(string documentType)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.DocumentType == documentType && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentNoAsync(string documentNo)
        {
            try
            {
                var entities = await _unitOfWork.SrtHeaders.FindAsync(x => x.ERPReferenceNumber == documentNo && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
                return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
                }
                if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.YearCode?.Length != 4)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
                if (createDto.PlannedDate == default)
                {
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
                }
               var entity = _mapper.Map<SrtHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.SrtHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderCreationError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<SrtHeaderDto>.ErrorResult(nf, nf, 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.SrtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SrtHeaderDto>(entity);
                return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderUpdateError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.SrtHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderDeletionError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SrtHeaders.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }
                entity.IsCompleted = true;
                entity.CompletionDate = DateTime.UtcNow;
                _unitOfWork.SrtHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderCompletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderCompletionError"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
