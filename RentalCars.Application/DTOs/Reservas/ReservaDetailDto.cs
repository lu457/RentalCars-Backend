using RentalCars.Application.DTOs.Resenas;

namespace RentalCars.Application.DTOs.Reservas;

    public record ReservaDetailDto : ReservaResponseDto
    {
        public ResenaResponseDto? Resena { get; init; }
    }


