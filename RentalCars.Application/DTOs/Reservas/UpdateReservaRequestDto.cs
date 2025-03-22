namespace RentalCars.Application.DTOs.Reservas;

    public record UpdateReservaRequestDto
    {
        public Guid Id { get; init; }  // ID de la reserva a actualizar
        public string Estado { get; init; } = string.Empty;  // Nuevo estado de la reserva (Pendiente, Confirmada, Cancelada)
    }


