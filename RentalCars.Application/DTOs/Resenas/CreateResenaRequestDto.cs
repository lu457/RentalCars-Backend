namespace RentalCars.Application.DTOs.Resenas
{
    public record CreateResenaRequestDto
    {
        public string Comentario { get; init; } = string.Empty; // Comentario de la reseña
        public int Calificacion { get; init; } // Calificación de la reseña (1-5)
        public Guid VehiculoId { get; init; } // ID del vehículo sobre el que se está dejando la reseña
        public Guid ReservaId { get; init; } // ID de la reserva asociada a la reseña (opcional)
    }
}
