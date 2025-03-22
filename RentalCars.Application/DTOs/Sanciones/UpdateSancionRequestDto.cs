namespace RentalCars.Application.DTOs.Sanciones;

public record UpdateSancionRequestDto
{
    public Guid Id { get; init; }
    public string Estado { get; init; } = string.Empty;
}

