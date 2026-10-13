using ControlElectricoObrera.Api.DTOs;
using ControlElectricoObrera.Api.DTOs.Catalog;
using ControlElectricoObrera.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlElectricoObrera.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(CategoryQueryService categoryQueryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CategorySummaryDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CategorySummaryDto>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await categoryQueryService.GetAllAsync(cancellationToken));

    [HttpGet("{slug}")]
    [ProducesResponseType<CategoryDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDetailDto>> GetBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        var category = await categoryQueryService.GetBySlugAsync(slug, cancellationToken);
        return category is null
            ? NotFound(ApiErrorResponse.Create("category_not_found", "The requested category was not found."))
            : Ok(category);
    }
}
