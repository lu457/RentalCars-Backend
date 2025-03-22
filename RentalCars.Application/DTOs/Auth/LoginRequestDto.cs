namespace RentalCars.Application.DTOs.Auth;

public record LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Contraseña { get; init; } = string.Empty;
}
