namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record CategoryDetailDto(
    long Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageStorageKey,
    CategorySummaryDto? ParentCategory,
    IReadOnlyList<CategorySummaryDto> ChildCategories,
    string? SeoTitle,
    string? SeoDescription);
