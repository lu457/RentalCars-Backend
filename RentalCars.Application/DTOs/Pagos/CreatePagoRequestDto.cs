namespace RentalCars.Application.DTOs.Pagos;

public record CreatePagoRequestDto
{
    public Guid ReservaId { get; init; }
    public decimal Monto { get; init; }
    public string MetodoPago { get; init; } = string.Empty;
}
