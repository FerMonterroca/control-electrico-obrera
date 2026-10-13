namespace ControlElectricoObrera.Api.Models;

public sealed class SpecialMaterialAttachment
{
    public long Id { get; set; }
    public long SpecialMaterialRequestId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public SpecialMaterialRequest SpecialMaterialRequest { get; set; } = null!;
}
