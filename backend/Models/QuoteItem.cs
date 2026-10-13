namespace ControlElectricoObrera.Api.Models;

public sealed class QuoteItem
{
    public long Id { get; set; }
    public long QuoteRequestId { get; set; }
    public long? ProductId { get; set; }
    public string ProductNameSnapshot { get; set; } = string.Empty;
    public string? BrandSnapshot { get; set; }
    public string? ModelSnapshot { get; set; }
    public string? SkuOrCodeSnapshot { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? CustomerNotes { get; set; }

    public QuoteRequest QuoteRequest { get; set; } = null!;
    public Product? Product { get; set; }
}
