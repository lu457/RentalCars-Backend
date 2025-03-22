using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Notificaciones;

namespace RentalCars.Application.Interfaces;

    public interface INotificacionService
    {
    /// Crea una notificación cuando se genera un pago o reserva.
    Task<Result<NotificacionResponseDto>> CrearNotificacionAsync(
        Guid usuarioId, string mensaje, CancellationToken cancellationToken);

    /// Obtiene todas las notificaciones de un usuario.
    Task<Result<List<NotificacionResponseDto>>> GetByUsuarioIdAsync(
        Guid usuarioId, CancellationToken cancellationToken = default);

    /// Obtiene todas las notificaciones no leídas de un usuario.
    Task<Result<List<NotificacionResponseDto>>> GetUnreadByUsuarioIdAsync(
        Guid usuarioId, CancellationToken cancellationToken = default);

    /// Marca una notificación como leída.
    Task<Result<bool>> MarkAsReadAsync(Guid notificacionId, CancellationToken cancellationToken = default);
}


