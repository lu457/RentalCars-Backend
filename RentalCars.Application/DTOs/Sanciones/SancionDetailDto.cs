using RentalCars.Application.DTOs.Reservas;

namespace RentalCars.Application.DTOs.Sanciones;

public record SancionDetailDto: SancionResponseDto
{
    public ReservaResponseDto? Reserva { get; init; }
}