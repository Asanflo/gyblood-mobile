namespace GyBlood.Mobile.Models.Requests;

public sealed record UpdateStocksRequest(
    List<StockItemRequest> Items);
