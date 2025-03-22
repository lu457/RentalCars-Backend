namespace RentalCars.Application.DTOs.Auth;

public record RegisterRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Contraseña { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Celular { get; init; } = string.Empty;
}
