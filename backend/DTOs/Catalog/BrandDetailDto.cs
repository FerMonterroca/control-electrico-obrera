namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record BrandDetailDto(
    long Id,
    string Name,
    string Slug,
    string? Description,
    string? LogoStorageKey,
    string? SeoTitle,
    string? SeoDescription);
