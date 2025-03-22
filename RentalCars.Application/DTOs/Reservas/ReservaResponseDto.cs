using RentalCars.Application.DTOs.Vehiculos;

namespace RentalCars.Application.DTOs.Reservas;

    public record ReservaResponseDto
    {
        public Guid Id { get; init; }  // ID de la reserva
        public DateTime FechaInicio { get; init; }  // Fecha de inicio de la reserva
        public DateTime FechaFin { get; init; }  // Fecha de fin de la reserva
        public string Comentario { get; init; } = string.Empty;
        public decimal PrecioTotal { get; init; }  // Precio total de la reserva
        public string Estado { get; init; } = string.Empty;  // Estado de la reserva (Pendiente, Confirmada, Cancelada)

        // Información del vehículo
        public VehiculoResponseDto Vehiculo { get; init; } = null!;
    }

