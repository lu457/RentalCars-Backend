using RentalCars.Application.DTOs.Resenas;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Application.DTOs.Vehiculos;


    public record VehiculoDetailDto : VehiculoResponseDto
    {
        // Información adicional sobre el propietario del vehículo
        public string PropietarioNombre { get; init; } = string.Empty;

        // Promedio de calificación del vehículo (si aplica)
        public double CalificacionPromedio { get; init; }

        // Lista de reseñas del vehículo
        public List<ResenaResponseDto> Resenas { get; init; } = [];

        // Lista de reservas realizadas para el vehículo
        public List<ReservaResponseDto> Reservas { get; init; } = [];
}

