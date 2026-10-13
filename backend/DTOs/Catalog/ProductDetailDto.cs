using ControlElectricoObrera.Api.Models;

namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record ProductDetailDto(
    long Id,
    string Name,
    string Slug,
    string? ShortDescription,
    string? Description,
    BrandSummaryDto? Brand,
    CategorySummaryDto Category,
    string? Model,
    string? SkuOrCode,
    string Unit,
    ProductAvailabilityStatus AvailabilityStatus,
    bool IsFeatured,
    string? SeoTitle,
    string? SeoDescription,
    IReadOnlyList<ProductImageDto> Images,
    IReadOnlyList<ProductSpecificationDto> Specifications,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
