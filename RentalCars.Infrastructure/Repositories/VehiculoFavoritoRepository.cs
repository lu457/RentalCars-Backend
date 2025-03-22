using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories
{
    public class VehiculoFavoritoRepository : BaseRepository<VehiculoFavorito>, IVehiculoFavoritoRepository
    {
        public VehiculoFavoritoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> EsFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(vf => vf.UsuarioId == usuarioId && vf.VehiculoId == vehiculoId, cancellationToken);
        }

        public async Task AgregarFavoritoAsync(VehiculoFavorito vehiculoFavorito, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(vehiculoFavorito, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoverFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken)
        {
            var favorito = await _dbSet.FirstOrDefaultAsync(vf => vf.UsuarioId == usuarioId && vf.VehiculoId == vehiculoId, cancellationToken);
            if (favorito != null)
            {
                _dbSet.Remove(favorito);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
