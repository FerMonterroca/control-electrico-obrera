using ControlElectricoObrera.Api.Data;
using ControlElectricoObrera.Api.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace ControlElectricoObrera.Api.Services;

public sealed class BrandQueryService(AppDbContext dbContext)
{
    public Task<List<BrandSummaryDto>> GetAllAsync(CancellationToken cancellationToken) =>
        dbContext.Brands
            .AsNoTracking()
            .Where(brand => brand.IsActive)
            .OrderBy(brand => brand.Name)
            .ThenBy(brand => brand.Id)
            .Select(brand => new BrandSummaryDto(brand.Id, brand.Name, brand.Slug))
            .ToListAsync(cancellationToken);

    public Task<BrandDetailDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken) =>
        dbContext.Brands
            .AsNoTracking()
            .Where(brand => brand.IsActive && brand.Slug == slug)
            .Select(brand => new BrandDetailDto(
                brand.Id,
                brand.Name,
                brand.Slug,
                brand.Description,
                brand.LogoStorageKey,
                brand.SeoTitle,
                brand.SeoDescription))
            .SingleOrDefaultAsync(cancellationToken);
}
