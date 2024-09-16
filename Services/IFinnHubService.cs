using StockView.Models;

namespace StockView.Services;

public interface IFinnHubService
{
    public Task<StockSymbol[]?> GetStockSymbolsAsync();
}