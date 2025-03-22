namespace RentalCars.Application.DTOs.Sanciones;

public record CreateSancionRequestDto
{
    public string Motivo { get; init; } = string.Empty;
    public decimal Monto { get; init; }
    public Guid ReservaId { get; init; }
}
