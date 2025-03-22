namespace RentalCars.Application.DTOs.Pagos;

public record UpdatePagoRequestDto
{
    public Guid Id { get; init; }
    public string Estado { get; init; } = string.Empty;
}
