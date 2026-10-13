using ControlElectricoObrera.Api.DTOs;
using ControlElectricoObrera.Api.DTOs.Catalog;
using ControlElectricoObrera.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlElectricoObrera.Api.Controllers;

[ApiController]
[Route("api/brands")]
public sealed class BrandsController(BrandQueryService brandQueryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<BrandSummaryDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BrandSummaryDto>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await brandQueryService.GetAllAsync(cancellationToken));

    [HttpGet("{slug}")]
    [ProducesResponseType<BrandDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrandDetailDto>> GetBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        var brand = await brandQueryService.GetBySlugAsync(slug, cancellationToken);
        return brand is null
            ? NotFound(ApiErrorResponse.Create("brand_not_found", "The requested brand was not found."))
            : Ok(brand);
    }
}
