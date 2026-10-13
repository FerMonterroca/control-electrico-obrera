namespace ControlElectricoObrera.Api.Models;

public sealed class ProductImage
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Product Product { get; set; } = null!;
}
