using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.Vehiculos)
            .Include(u => u.Reservas)
            .Include(u => u.Resenas)
            .Include(u => u.VehiculoFavoritos)
            .Include(u => u.Notificaciones)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // Actualiza la última fecha de ingreso del usuario.
    public async Task<bool> UpdateLastLoginAsync(
        Guid usuarioId,
        CancellationToken cancellationToken = default
    )
    {
        var usuario = await _dbSet.FirstOrDefaultAsync(u => u.Id == usuarioId, cancellationToken);

        if (usuario == null)
        {
            return false;
        }

        var updatedUser = usuario with { UltimaFechaDeIngreso = DateTime.Now };

        _dbSet.Entry(usuario).State = EntityState.Detached;
        _dbSet.Update(updatedUser);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}