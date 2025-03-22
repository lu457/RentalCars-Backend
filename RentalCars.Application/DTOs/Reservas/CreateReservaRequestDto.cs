namespace RentalCars.Application.DTOs.Reservas;

    public record CreateReservaRequestDto
    {
        public DateTime FechaInicio { get; init; }  // Fecha de inicio de la reserva
        public DateTime FechaFin { get; init; }  // Fecha de fin de la reserva
        public string Comentario { get; init; } = string.Empty;
        public Guid VehiculoId { get; init; }  // ID del vehículo a reservar
    }


