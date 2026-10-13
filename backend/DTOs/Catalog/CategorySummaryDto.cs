namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record CategorySummaryDto(
    long Id,
    string Name,
    string Slug,
    long? ParentCategoryId);
