using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces
{
    public interface INotificacionRepository : IRepository<Notificacion>
    {
        /// Obtiene todas las notificaciones de un usuario ordenadas por fecha de creación.
        Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);

        /// Obtiene todas las notificaciones NO leídas de un usuario.
        Task<IEnumerable<Notificacion>> GetUnreadByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);

        /// Marca una notificación como leída.
        Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

