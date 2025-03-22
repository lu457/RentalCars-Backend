namespace RentalCars.Application.DTOs.Vehiculos;

public record ImageResponseDto
{
    public string Url { get; init; } = string.Empty;
    public bool EsPrincipal { get; init; }
}
