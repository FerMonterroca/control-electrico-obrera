using ControlElectricoObrera.Api.DTOs;
using ControlElectricoObrera.Api.Models;

namespace ControlElectricoObrera.Api.Services;

public sealed record ProductQueryOptions(
    int Page,
    int PageSize,
    string? Search,
    string? BrandSlug,
    string? CategorySlug,
    ProductAvailabilityStatus? Availability,
    bool? Featured,
    ProductSort Sort)
{
    public const int DefaultPageSize = 24;
    public const int MaximumPageSize = 100;
    private const int MaximumPage = 1_000_000;

    public static bool TryCreate(
        string? page,
        string? pageSize,
        string? search,
        string? brand,
        string? category,
        string? availability,
        string? featured,
        string? sort,
        out ProductQueryOptions? options,
        out ApiErrorResponse? error)
    {
        options = null;
        error = null;

        if (!TryParseInteger(page, 1, out var parsedPage) || parsedPage > MaximumPage)
        {
            error = ApiErrorResponse.Create(
                "invalid_page",
                $"Page must be between 1 and {MaximumPage}.");
            return false;
        }

        if (!TryParseInteger(pageSize, DefaultPageSize, out var parsedPageSize)
            || parsedPageSize > MaximumPageSize)
        {
            error = ApiErrorResponse.Create(
                "invalid_page_size",
                $"Page size must be between 1 and {MaximumPageSize}.");
            return false;
        }

        ProductAvailabilityStatus? parsedAvailability = null;
        if (!string.IsNullOrWhiteSpace(availability))
        {
            if (!Enum.TryParse<ProductAvailabilityStatus>(availability, true, out var value)
                || !Enum.IsDefined(value))
            {
                error = ApiErrorResponse.Create(
                    "invalid_availability",
                    "The requested availability value is invalid.");
                return false;
            }

            parsedAvailability = value;
        }

        bool? parsedFeatured = null;
        if (!string.IsNullOrWhiteSpace(featured))
        {
            if (!bool.TryParse(featured, out var value))
            {
                error = ApiErrorResponse.Create(
                    "invalid_featured",
                    "Featured must be either true or false.");
                return false;
            }

            parsedFeatured = value;
        }

        if (!TryParseSort(sort, out var parsedSort))
        {
            error = ApiErrorResponse.Create(
                "invalid_sort",
                "Sort must be one of: name-asc, name-desc, newest.");
            return false;
        }

        var normalizedSearch = NormalizeOptional(search);
        var normalizedBrand = NormalizeOptional(brand);
        var normalizedCategory = NormalizeOptional(category);

        if (normalizedSearch?.Length > 200)
        {
            error = ApiErrorResponse.Create("invalid_search", "Search cannot exceed 200 characters.");
            return false;
        }

        if (normalizedBrand?.Length > 220 || normalizedCategory?.Length > 220)
        {
            error = ApiErrorResponse.Create("invalid_filter", "Brand and category filters cannot exceed 220 characters.");
            return false;
        }

        options = new ProductQueryOptions(
            parsedPage,
            parsedPageSize,
            normalizedSearch,
            normalizedBrand,
            normalizedCategory,
            parsedAvailability,
            parsedFeatured,
            parsedSort);
        return true;
    }

    private static bool TryParseInteger(string? input, int defaultValue, out int value)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = defaultValue;
            return true;
        }

        return int.TryParse(input, out value) && value >= 1;
    }

    private static bool TryParseSort(string? input, out ProductSort sort)
    {
        switch (input?.Trim().ToLowerInvariant())
        {
            case null or "" or "newest":
                sort = ProductSort.Newest;
                return true;
            case "name-asc":
                sort = ProductSort.NameAscending;
                return true;
            case "name-desc":
                sort = ProductSort.NameDescending;
                return true;
            default:
                sort = default;
                return false;
        }
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public enum ProductSort
{
    Newest,
    NameAscending,
    NameDescending
}
