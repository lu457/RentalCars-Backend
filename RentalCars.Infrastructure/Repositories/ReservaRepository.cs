using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Reserva>> GetReservasByVehiculoIdAsync(Guid vehiculoId)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Vehiculo)
                .Where(r => r.VehiculoId == vehiculoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet
                .Include(r => r.Vehiculo)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<bool> IsVehiculoDisponibleAsync(Guid vehiculoId, DateTime fechaInicio, DateTime fechaFin)
        {
            return !await _dbSet
                .Where(r => r.VehiculoId == vehiculoId)
                .Where(r => r.Estado != EstadoReserva.Cancelada)
                .AnyAsync(r => fechaInicio < r.FechaFin && fechaFin > r.FechaInicio);
        }
        public override async Task<Reserva?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
        .Include(r => r.Usuario)
        .Include(r => r.Vehiculo)
        .Include(r => r.Resena)
        .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }


    }
}
