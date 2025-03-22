namespace RentalCars.Application.DTOs.Auth;

public record AuthResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string NombreCompleto { get; init; } = string.Empty;
}
