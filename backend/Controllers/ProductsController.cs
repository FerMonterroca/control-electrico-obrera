using ControlElectricoObrera.Api.DTOs;
using ControlElectricoObrera.Api.DTOs.Catalog;
using ControlElectricoObrera.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlElectricoObrera.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductQueryService productQueryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResponse<ProductListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<ProductListItemDto>>> GetAll(
        [FromQuery] string? page,
        [FromQuery] string? pageSize,
        [FromQuery] string? search,
        [FromQuery] string? brand,
        [FromQuery] string? category,
        [FromQuery] string? availability,
        [FromQuery] string? featured,
        [FromQuery] string? sort,
        CancellationToken cancellationToken)
    {
        if (!ProductQueryOptions.TryCreate(
                page,
                pageSize,
                search,
                brand,
                category,
                availability,
                featured,
                sort,
                out var options,
                out var error))
        {
            return BadRequest(error);
        }

        return Ok(await productQueryService.GetProductsAsync(options!, cancellationToken));
    }

    [HttpGet("{slug}")]
    [ProducesResponseType<ProductDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailDto>> GetBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        var product = await productQueryService.GetBySlugAsync(slug, cancellationToken);
        return product is null
            ? NotFound(ApiErrorResponse.Create("product_not_found", "The requested product was not found."))
            : Ok(product);
    }
}
