using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories
{
    public class ResenaRepository : BaseRepository<Resena>, IResenaRepository
    {
        public ResenaRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Resena>> GetResenasByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(rn => rn.Critico)
                .Where(rn => rn.VehiculoId == vehiculoId)
                .OrderByDescending(rn => rn.FechaResena)
                .ToListAsync();
        }
        public async Task<double> GetAverageRatingByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(rn => rn.VehiculoId == vehiculoId)
                .AverageAsync(rn => rn.Calificacion);
        }
        public async Task<IEnumerable<Resena>> GetByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(rn => rn.VehiculoId == vehiculoId)
                .Include(rn => rn.Critico)
                .OrderByDescending(rn => rn.FechaResena)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Resena?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(rn => rn.Critico)
                .FirstOrDefaultAsync(rn => rn.Id == id, cancellationToken);
        }

        public async Task<Resena?> GetByVehiculoAndCriticoAsync(Guid vehiculoId, Guid criticoId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(rn => rn.VehiculoId == vehiculoId && rn.CriticoId == criticoId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Resena?> GetByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(rn => rn.ReservaId == reservaId)
                .Include(rn => rn.Critico)
                .Include(rn => rn.Vehiculo)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

