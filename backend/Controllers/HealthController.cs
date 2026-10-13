using ControlElectricoObrera.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ControlElectricoObrera.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    private const string ServiceName = "Control Electrico Obrera API";

    [HttpGet]
    [ProducesResponseType<HealthResponse>(StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse("ok", ServiceName));
    }
}
