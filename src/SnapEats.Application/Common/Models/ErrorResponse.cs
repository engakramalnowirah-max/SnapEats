namespace SnapEats.Application.Common.Models;

public sealed record ErrorResponse(
    string Title,
    int StatusCode,
    string? Detail = null,
    IReadOnlyDictionary<string, string[]>? Errors = null);

