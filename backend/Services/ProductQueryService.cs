using ControlElectricoObrera.Api.Data;
using ControlElectricoObrera.Api.DTOs.Catalog;
using ControlElectricoObrera.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlElectricoObrera.Api.Services;

public sealed class ProductQueryService(AppDbContext dbContext)
{
    public async Task<PagedResponse<ProductListItemDto>> GetProductsAsync(
        ProductQueryOptions options,
        CancellationToken cancellationToken)
    {
        var query = PublicProducts();

        if (options.Search is not null)
        {
            var pattern = $"%{EscapeLikePattern(options.Search)}%";
            query = query.Where(product =>
                EF.Functions.ILike(product.Name, pattern, "\\")
                || (product.Model != null && EF.Functions.ILike(product.Model, pattern, "\\"))
                || (product.SkuOrCode != null && EF.Functions.ILike(product.SkuOrCode, pattern, "\\"))
                || (product.Brand != null && EF.Functions.ILike(product.Brand.Name, pattern, "\\")));
        }

        if (options.BrandSlug is not null)
        {
            query = query.Where(product =>
                product.Brand != null && product.Brand.Slug == options.BrandSlug);
        }

        if (options.CategorySlug is not null)
        {
            query = query.Where(product => product.Category.Slug == options.CategorySlug);
        }

        if (options.Availability is not null)
        {
            query = query.Where(product => product.AvailabilityStatus == options.Availability);
        }

        if (options.Featured is not null)
        {
            query = query.Where(product => product.IsFeatured == options.Featured);
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var orderedQuery = options.Sort switch
        {
            ProductSort.NameAscending => query.OrderBy(product => product.Name).ThenBy(product => product.Id),
            ProductSort.NameDescending => query.OrderByDescending(product => product.Name).ThenBy(product => product.Id),
            _ => query.OrderByDescending(product => product.CreatedAt).ThenByDescending(product => product.Id)
        };

        var items = await orderedQuery
            .Skip((options.Page - 1) * options.PageSize)
            .Take(options.PageSize)
            .Select(product => new ProductListItemDto(
                product.Id,
                product.Name,
                product.Slug,
                product.ShortDescription,
                product.Brand == null
                    ? null
                    : new BrandSummaryDto(product.Brand.Id, product.Brand.Name, product.Brand.Slug),
                new CategorySummaryDto(
                    product.Category.Id,
                    product.Category.Name,
                    product.Category.Slug,
                    product.Category.ParentCategoryId),
                product.Model,
                product.SkuOrCode,
                product.Unit,
                product.AvailabilityStatus,
                product.IsFeatured,
                product.Images
                    .OrderByDescending(image => image.IsPrimary)
                    .ThenBy(image => image.SortOrder)
                    .Select(image => image.StorageKey)
                    .FirstOrDefault(),
                product.CreatedAt))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)options.PageSize);

        return new PagedResponse<ProductListItemDto>(
            items,
            options.Page,
            options.PageSize,
            totalItems,
            totalPages);
    }

    public async Task<ProductDetailDto?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken)
    {
        var product = await PublicProducts()
            .Where(product => product.Slug == slug)
            .Select(product => new ProductDetailDto(
                product.Id,
                product.Name,
                product.Slug,
                product.ShortDescription,
                product.Description,
                product.Brand == null
                    ? null
                    : new BrandSummaryDto(product.Brand.Id, product.Brand.Name, product.Brand.Slug),
                new CategorySummaryDto(
                    product.Category.Id,
                    product.Category.Name,
                    product.Category.Slug,
                    product.Category.ParentCategoryId),
                product.Model,
                product.SkuOrCode,
                product.Unit,
                product.AvailabilityStatus,
                product.IsFeatured,
                product.SeoTitle,
                product.SeoDescription,
                Array.Empty<ProductImageDto>(),
                Array.Empty<ProductSpecificationDto>(),
                product.CreatedAt,
                product.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return null;
        }

        var images = await dbContext.ProductImages
            .AsNoTracking()
            .Where(image => image.ProductId == product.Id)
            .OrderBy(image => image.SortOrder)
            .ThenBy(image => image.Id)
            .Select(image => new ProductImageDto(
                image.StorageKey,
                image.AltText,
                image.SortOrder,
                image.IsPrimary))
            .ToListAsync(cancellationToken);

        var specifications = await dbContext.ProductSpecifications
            .AsNoTracking()
            .Where(specification => specification.ProductId == product.Id)
            .OrderBy(specification => specification.SortOrder)
            .ThenBy(specification => specification.Id)
            .Select(specification => new ProductSpecificationDto(
                specification.Name,
                specification.Value,
                specification.Unit,
                specification.SortOrder))
            .ToListAsync(cancellationToken);

        return product with { Images = images, Specifications = specifications };
    }

    private IQueryable<Product> PublicProducts() =>
        dbContext.Products
            .AsNoTracking()
            .Where(product =>
                product.IsActive
                && product.Category.IsActive
                && (product.BrandId == null || product.Brand!.IsActive));

    private static string EscapeLikePattern(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
}
