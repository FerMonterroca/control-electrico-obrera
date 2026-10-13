using ControlElectricoObrera.Api.Models;

namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record ProductListItemDto(
    long Id,
    string Name,
    string Slug,
    string? ShortDescription,
    BrandSummaryDto? Brand,
    CategorySummaryDto Category,
    string? Model,
    string? SkuOrCode,
    string Unit,
    ProductAvailabilityStatus AvailabilityStatus,
    bool IsFeatured,
    string? PrimaryImageStorageKey,
    DateTimeOffset CreatedAt);
