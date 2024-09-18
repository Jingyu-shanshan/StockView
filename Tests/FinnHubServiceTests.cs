using System.Net;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using StockView.Models;
using StockView.Services.Impl;
using Xunit;

namespace StockView.Tests;

public class FinnHubServiceTests
{
    [Fact]
    public async Task GetStockSymbolAsync_ShouldReturnValidSymbolArray_WhenFinnHubApiCallSuccessful()
    {
        const string filePath = "Tests/TestData/FinnHubApiResponseData.json";

        var finnHubParam = new FinnHubParam
        {
            Exchange = "USD",
            Mic = "XNYS"
        };


        var jsonContent = await File.ReadAllTextAsync(filePath);
        
        var appSettings = new FinnHub
        {
            ApiKey = "TestApiKey"
        };
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent)
            });
        
        var mockOptions = new Mock<IOptions<FinnHub>>();
        mockOptions.Setup(o => o.Value).Returns(appSettings);
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var finnHubService = new FinnHubService(mockOptions.Object, httpClient);
        
        var stockSymbols = await finnHubService.GetStockSymbolsAsync(finnHubParam);
        
        Assert.NotNull(stockSymbols);
        Assert.Equal(3, stockSymbols.Length);
        Assert.Equal("USD", stockSymbols[0].Currency);
    }
    
    [Fact]
    public async Task GetStockSymbolAsync_ShouldReturnNull_WhenFinnHubApiReturnsUnsuccessfulStateCode()
    {
        const string jsonContent = "Error";
        
        var appSettings = new FinnHub
        {
            ApiKey = "TestApiKey"
        };
        
        var finnHubParam = new FinnHubParam
        {
            Exchange = "USD",
            Mic = "XNYS"
        };
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(jsonContent)
            });
        
        var mockOptions = new Mock<IOptions<FinnHub>>();
        mockOptions.Setup(o => o.Value).Returns(appSettings);
        
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var finnHubService = new FinnHubService(mockOptions.Object, httpClient);
        
        var stockSymbols = await finnHubService.GetStockSymbolsAsync(finnHubParam);
        
        Assert.Null(stockSymbols);
    }
}