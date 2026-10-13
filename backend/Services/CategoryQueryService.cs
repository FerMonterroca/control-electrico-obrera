using ControlElectricoObrera.Api.Data;
using ControlElectricoObrera.Api.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace ControlElectricoObrera.Api.Services;

public sealed class CategoryQueryService(AppDbContext dbContext)
{
    public Task<List<CategorySummaryDto>> GetAllAsync(CancellationToken cancellationToken) =>
        dbContext.Categories
            .AsNoTracking()
            .Where(category => category.IsActive)
            .OrderBy(category => category.Name)
            .ThenBy(category => category.Id)
            .Select(category => new CategorySummaryDto(
                category.Id,
                category.Name,
                category.Slug,
                category.ParentCategory != null && category.ParentCategory.IsActive
                    ? category.ParentCategoryId
                    : null))
            .ToListAsync(cancellationToken);

    public async Task<CategoryDetailDto?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .Where(category => category.IsActive && category.Slug == slug)
            .Select(category => new CategoryDetailDto(
                category.Id,
                category.Name,
                category.Slug,
                category.Description,
                category.ImageStorageKey,
                null,
                Array.Empty<CategorySummaryDto>(),
                category.SeoTitle,
                category.SeoDescription))
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return null;
        }

        var parent = await dbContext.Categories
            .AsNoTracking()
            .Where(candidate => candidate.IsActive
                && candidate.ChildCategories.Any(child => child.Id == category.Id))
            .Select(candidate => new CategorySummaryDto(
                candidate.Id,
                candidate.Name,
                candidate.Slug,
                candidate.ParentCategoryId))
            .SingleOrDefaultAsync(cancellationToken);

        var children = await dbContext.Categories
            .AsNoTracking()
            .Where(child => child.IsActive && child.ParentCategoryId == category.Id)
            .OrderBy(child => child.Name)
            .ThenBy(child => child.Id)
            .Select(child => new CategorySummaryDto(
                child.Id,
                child.Name,
                child.Slug,
                child.ParentCategoryId))
            .ToListAsync(cancellationToken);

        return category with { ParentCategory = parent, ChildCategories = children };
    }
}
