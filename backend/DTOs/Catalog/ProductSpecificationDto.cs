namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record ProductSpecificationDto(
    string Name,
    string Value,
    string? Unit,
    int SortOrder);
