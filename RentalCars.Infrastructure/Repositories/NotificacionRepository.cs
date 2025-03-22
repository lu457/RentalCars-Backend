using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RentalCars.Domain.Enums;

namespace RentalCars.Infrastructure.Repositories;

public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
{
    public NotificacionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Notificacion>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notificacion>> GetUnreadByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(n => n.UsuarioId == usuarioId && n.Estado == EstadoNotificacion.NoLeido)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var notificacion = await _dbSet.FindAsync(id, cancellationToken);
        if (notificacion != null)
        {
            notificacion.Estado = EstadoNotificacion.Leido;
            await UpdateAsync(notificacion, cancellationToken);
        }
    }
}


