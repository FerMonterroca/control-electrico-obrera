namespace ControlElectricoObrera.Api.DTOs.Catalog;

public sealed record ProductImageDto(
    string StorageKey,
    string AltText,
    int SortOrder,
    bool IsPrimary);
