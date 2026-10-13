namespace ControlElectricoObrera.Api.Models;

public sealed class SpecialMaterialRequest
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public string CustomerNameSnapshot { get; set; } = string.Empty;
    public string CustomerPhoneSnapshot { get; set; } = string.Empty;
    public string? CustomerEmailSnapshot { get; set; }
    public string? CompanyNameSnapshot { get; set; }
    public string? ProductName { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Code { get; set; }
    public decimal? Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Urgency { get; set; } = string.Empty;
    public SpecialMaterialRequestStatus Status { get; set; } = SpecialMaterialRequestStatus.New;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Customer Customer { get; set; } = null!;
    public ICollection<SpecialMaterialAttachment> Attachments { get; set; } = new List<SpecialMaterialAttachment>();
}
