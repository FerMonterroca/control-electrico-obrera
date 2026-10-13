namespace ControlElectricoObrera.Api.DTOs;

public sealed record ApiErrorResponse(ApiError Error)
{
    public static ApiErrorResponse Create(string code, string message) =>
        new(new ApiError(code, message));
}

public sealed record ApiError(string Code, string Message);
