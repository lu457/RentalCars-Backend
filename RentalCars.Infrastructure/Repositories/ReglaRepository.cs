using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories;

public class ReglaRepository : BaseRepository<Regla>, IReglaRepository
{
    public ReglaRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Regla?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.VehiculoReglas)
            .ThenInclude(vr => vr.Vehiculo)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Regla?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Nombre == nombre, cancellationToken);
    }

    public async Task AddVehiculoReglaAsync(
        Guid vehiculoId,
        Guid reglaId,
        CancellationToken cancellationToken = default)
    {
        var regla = await GetByIdAsync(reglaId, cancellationToken);
        if (regla == null)
            throw new InvalidOperationException("Regla no encontrada");

        var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Id == vehiculoId, cancellationToken);
        if (vehiculo == null)
            throw new InvalidOperationException("Vehículo no encontrado");

    }
}

