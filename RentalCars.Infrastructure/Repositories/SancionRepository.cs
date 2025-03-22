using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RentalCars.Domain.Enums;

namespace RentalCars.Infrastructure.Repositories
{
    public class SancionRepository : BaseRepository<Sancion>, ISancionRepository
    {
        public SancionRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Sancion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.Reserva)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Sancion?> GetByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.Reserva)
                .FirstOrDefaultAsync(s => s.ReservaId == reservaId, cancellationToken);
        }
        public async Task<IEnumerable<Sancion>> GetSancionesActivasAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.Reserva)
                .Where(s => s.Estado == EstadoSancion.Pendiente)
                .ToListAsync(cancellationToken);
        }
    }
}

