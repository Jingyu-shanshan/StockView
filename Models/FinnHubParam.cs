using System.ComponentModel.DataAnnotations;

namespace StockView.Models;

public class FinnHubParam
{
    [Required(ErrorMessage = "Exchange is required.")]
    public required string Exchange { get; set; }
    public string? Mic { get; set; }
    public string? SecurityType { get; set; }
    public string? Currency { get; set; }
}