namespace RentalCars.Application.DTOs.Resenas;

    public record ResenaResponseDto
    {
        public Guid Id { get; init; }  // ID de la reseña
        public string Comentario { get; init; } = string.Empty;  // Comentario de la reseña
        public int Calificacion { get; init; }  // Calificación de la reseña (por ejemplo, de 1 a 5)
        public DateTime FechaResena { get; init; }

    // Información del crítico (usuario que hizo la reseña)
        public string CriticoNombre { get; init; } = string.Empty;  // Nombre del crítico (usuario)
        public Guid VehiculoId { get; init; }  // ID del crítico (usuario)
    }


