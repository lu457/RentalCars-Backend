namespace RentalCars.Application.DTOs.Sanciones;

public record SancionResponseDto
{
    public Guid Id { get; init; }
    public string Motivo { get; init; } = string.Empty;
    public decimal Monto { get; init; }
    public DateTime FechaSancion { get; init; }
    public string Estado { get; init; } = string.Empty;
    public Guid ReservaId { get; init; }
}

