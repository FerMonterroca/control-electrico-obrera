namespace ControlElectricoObrera.Api.Models;

public sealed class SiteSettings
{
    public long Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }
    public string? GeneralLocation { get; set; }
    public string? BusinessHours { get; set; }
    public bool TwentyFourSevenRequestsEnabled { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
