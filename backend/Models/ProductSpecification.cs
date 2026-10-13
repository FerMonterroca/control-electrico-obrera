namespace ControlElectricoObrera.Api.Models;

public sealed class ProductSpecification
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public int SortOrder { get; set; }

    public Product Product { get; set; } = null!;
}
