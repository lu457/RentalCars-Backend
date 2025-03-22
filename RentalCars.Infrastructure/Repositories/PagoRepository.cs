using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Pago>> GetPagosByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Reserva)
                .Where(p => p.ReservaId == reservaId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistePagoParaReservaAsync(Guid reservaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(p => p.ReservaId == reservaId, cancellationToken);
        }

        public async Task<IEnumerable<Pago>> GetUserPagosAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Reserva)
                .Where(p => p.Reserva.UsuarioId == usuarioId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Pago>> GetPagosByPropietarioAsync(Guid propietarioId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Reserva)
                .ThenInclude(r => r.Vehiculo)
                .Where(p => p.Reserva.Vehiculo.PropietarioId == propietarioId)
                .ToListAsync(cancellationToken);
        }
    }
}

