namespace ControlElectricoObrera.Api.Models;

public sealed class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public long? BrandId { get; set; }
    public long CategoryId { get; set; }
    public string? Model { get; set; }
    public string? SkuOrCode { get; set; }
    public string Unit { get; set; } = string.Empty;
    public ProductAvailabilityStatus AvailabilityStatus { get; set; } = ProductAvailabilityStatus.Consult;
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Brand? Brand { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
    public ICollection<QuoteItem> QuoteItems { get; set; } = new List<QuoteItem>();
}
