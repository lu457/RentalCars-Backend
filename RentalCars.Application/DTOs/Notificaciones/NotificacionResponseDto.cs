
namespace RentalCars.Application.DTOs.Notificaciones;

    /// <summary>
    /// DTO para representar una notificación en las respuestas del sistema.
    /// </summary>
    public record NotificacionResponseDto
    {
        /// <summary>
        /// Identificador único de la notificación.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Mensaje de la notificación.
        /// </summary>
        public string Mensaje { get; init; } = string.Empty;

        /// <summary>
        /// Estado de la notificación (Leída o No Leída).
        /// </summary>
        public string Estado { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de creación de la notificación.
    /// </summary>
        public DateTime FechaCreacion { get; init; }
    }


