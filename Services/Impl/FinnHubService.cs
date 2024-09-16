using System.Text.Json;
using Microsoft.Extensions.Options;
using StockView.Models;

namespace StockView.Services.Impl;

public class FinnHubService : IFinnHubService
{
    private const string BaseUrl = "https://finnhub.io/api/v1/stock/symbol";
    private readonly string? _apiKey;
    private readonly HttpClient _httpClient;
    
    public FinnHubService(IOptions<FinnHub> finnHubOptions, HttpClient httpClient)
    {
        _apiKey = finnHubOptions.Value.ApiKey;
        _httpClient = httpClient;
    }

    public async Task<StockSymbol[]?> GetStockSymbolsAsync()
    {
        StockSymbol[]? stockSymbols;
        
        try
        {
            var response = await _httpClient.GetAsync(
                BaseUrl + "?token=" + _apiKey + "&exchange=US&mic=XNYS");
        
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                stockSymbols = JsonSerializer.Deserialize<StockSymbol[]>(content);
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return stockSymbols;
    }
}