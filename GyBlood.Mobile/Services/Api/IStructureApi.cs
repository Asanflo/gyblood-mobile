using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IStructureApi
{
    [Get(ApiRoutes.Structures.Me)]
    Task<StructureDto> GetMeAsync();

    [Put(ApiRoutes.Structures.Me)]
    Task<StructureDto> UpdateMeAsync([Body] UpdateStructureRequest request);

    [Get(ApiRoutes.Structures.List)]
    Task<List<StructureDto>> GetStructuresAsync();

    [Get(ApiRoutes.Structures.Stocks)]
    Task<List<StockDto>> GetStocksAsync(BloodGroup? bloodGroup = null, Rhesus? rhesus = null, double? lat = null, double? lon = null);

    [Put(ApiRoutes.Structures.MyStocks)]
    Task<List<StockDto>> UpdateMyStocksAsync([Body] UpdateStocksRequest request);
}
