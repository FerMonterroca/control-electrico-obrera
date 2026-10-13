using ControlElectricoObrera.Api.Data;
using ControlElectricoObrera.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ControlElectricoObrera.Api.Controllers;

[ApiController]
[Route("api/ready")]
public sealed class ReadyController(
    AppDbContext dbContext,
    ILogger<ReadyController> logger) : ControllerBase
{
    private const string ServiceName = "Control Electrico Obrera API";

    [HttpGet]
    [ProducesResponseType<HealthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<HealthResponse>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<HealthResponse>> Get(CancellationToken cancellationToken)
    {
        bool canConnect;

        try
        {
            canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogWarning(
                "Database readiness check failed with {ExceptionType}.",
                exception.GetType().Name);
            canConnect = false;
        }

        if (!canConnect)
        {
            logger.LogWarning("Database readiness check reports unavailable.");
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new HealthResponse("unavailable", ServiceName));
        }

        return Ok(new HealthResponse("ok", ServiceName));
    }
}
