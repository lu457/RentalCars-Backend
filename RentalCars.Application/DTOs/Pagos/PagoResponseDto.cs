namespace RentalCars.Application.DTOs.Pagos;

public record PagoResponseDto
{
    public Guid Id { get; init; }
    public Guid ReservaId { get; init; }
    public decimal Monto { get; init; }
    public string MetodoPago { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
}
