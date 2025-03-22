

namespace RentalCars.Infrastructure.Authentication.Models;
public record AuthResult
{
    public bool Success { get; init; }
    public string Token { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
