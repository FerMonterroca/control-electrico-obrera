namespace ControlElectricoObrera.Api.Models;

public sealed class QuoteRequest
{
    public long Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public long CustomerId { get; set; }
    public string CustomerNameSnapshot { get; set; } = string.Empty;
    public string CustomerPhoneSnapshot { get; set; } = string.Empty;
    public string? CustomerEmailSnapshot { get; set; }
    public string? CompanyNameSnapshot { get; set; }
    public QuoteStatus Status { get; set; } = QuoteStatus.New;
    public string? PostalCode { get; set; }
    public DateOnly? NeededByDate { get; set; }
    public string? Comments { get; set; }
    public QuoteSource Source { get; set; } = QuoteSource.Catalog;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public ICollection<QuoteItem> Items { get; set; } = new List<QuoteItem>();
    public ICollection<QuoteAttachment> Attachments { get; set; } = new List<QuoteAttachment>();
}
