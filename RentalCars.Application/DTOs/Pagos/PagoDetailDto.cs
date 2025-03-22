using RentalCars.Application.DTOs.Reservas;

namespace RentalCars.Application.DTOs.Pagos;

public record PagoDetailDto : PagoResponseDto
{
    public ReservaResponseDto? Reserva { get; init; }
}
