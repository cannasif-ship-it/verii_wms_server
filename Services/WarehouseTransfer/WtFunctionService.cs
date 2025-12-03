using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Services
{
    public class WtFunctionService : IWtFunctionService
    {
        private readonly WmsDbContext _wmsDbContext;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WtFunctionService(WmsDbContext wmsDbContext, IMapper mapper, ILocalizationService localizationService)
        {
            _wmsDbContext = wmsDbContext;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<List<TransferOpenOrderHeaderDto>>> GetTransferOpenOrderHeaderAsync(string customerCode)
        {
            try
            {
                var headers = await _wmsDbContext.Set<FN_TransferOpenOrder_Header>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_TransferOpenOrder_Header({0})", customerCode)
                    .AsNoTracking()
                    .ToListAsync();

                var headerDtos = _mapper.Map<List<TransferOpenOrderHeaderDto>>(headers);

                return ApiResponse<List<TransferOpenOrderHeaderDto>>.SuccessResult(
                    headerDtos,
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderHeaderRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderHeaderRetrievalError"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }

        public async Task<ApiResponse<List<TransferOpenOrderLineDto>>> GetTransferOpenOrderLineAsync(string siparisNoCsv)
        {
            try
            {
                var lines = await _wmsDbContext.Set<FN_TransferOpenOrder_Line>()
                    .FromSqlRaw("SELECT * FROM dbo.RII_FN_TransferOpenOrder_Line({0})", siparisNoCsv)
                    .AsNoTracking()
                    .ToListAsync();

                var lineDtos = _mapper.Map<List<TransferOpenOrderLineDto>>(lines);

                return ApiResponse<List<TransferOpenOrderLineDto>>.SuccessResult(
                    lineDtos,
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderLineRetrievedSuccessfully")
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransferOpenOrderLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WtFunctionTransferOpenOrderLineRetrievalError"),
                    ex.Message ?? string.Empty,
                    500
                );
            }
        }
    }
}