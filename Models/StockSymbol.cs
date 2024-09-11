namespace StockView.Models;

public class StockSymbol
{
    public required string Currency { get; set; }
    public required string Description { get; set; }
    public required string DisplaySymbol { get; set; }
    public required string Figi { get; set; }
    public required string Isin { get; set; }
    public required string Mic { get; set; }
    public required string ShareClassFIGI { get; set; }
    public required string Symbol { get; set; }
    public required string Symbol2 { get; set; }
    public required string Type { get; set; }
}