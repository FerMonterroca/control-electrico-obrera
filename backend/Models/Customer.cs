namespace ControlElectricoObrera.Api.Models;

public sealed class Customer
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<QuoteRequest> QuoteRequests { get; set; } = new List<QuoteRequest>();
    public ICollection<SpecialMaterialRequest> SpecialMaterialRequests { get; set; } = new List<SpecialMaterialRequest>();
}
