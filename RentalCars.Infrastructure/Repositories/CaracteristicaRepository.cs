using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories;

public class CaracteristicaRepository : BaseRepository<Caracteristica>, ICaracteristicaRepository
{
    public CaracteristicaRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Caracteristica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet 
        .Include(c => c.VehiculoCaracteristicas)
            .ThenInclude(vc => vc.Vehiculo)
        .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    // Método para obtener una característica por su nombre.
    public async Task<Caracteristica?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Nombre == nombre, cancellationToken);
    }

    // Verificar si una característica ya está asociada a un vehículo
    public async Task<bool> ExistsVehiculoCaracteristicaAsync(
        Guid vehiculoId,
        Guid caracteristicaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.VehiculoCaracteristicas
            .AnyAsync(vc => vc.VehiculoId == vehiculoId && vc.CaracteristicaId == caracteristicaId, cancellationToken);
    }

    public async Task<IEnumerable<Caracteristica>> GetByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default)
    {
        return await _context.VehiculoCaracteristicas
            .Where(vc => vc.VehiculoId == vehiculoId)
            .Select(vc => vc.Caracteristica)
            .ToListAsync(cancellationToken);
    }

    // Agregar una característica a un vehículo
    public async Task AddVehiculoCaracteristicaAsync(
        Guid vehiculoId,
        Guid caracteristicaId,
        CancellationToken cancellationToken = default)
    {
        var vehiculoCaracteristica = new VehiculoCaracteristica
        {
            VehiculoId = vehiculoId,
            CaracteristicaId = caracteristicaId
        };

        _context.VehiculoCaracteristicas.Add(vehiculoCaracteristica);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // Eliminar una característica de un vehículo
    public async Task RemoveVehiculoCaracteristicaAsync(
        Guid vehiculoId,
        Guid caracteristicaId,
        CancellationToken cancellationToken = default)
    {
        var vehiculoCaracteristica = await _context.VehiculoCaracteristicas
            .FirstOrDefaultAsync(vc => vc.VehiculoId == vehiculoId && vc.CaracteristicaId == caracteristicaId, cancellationToken);

        if (vehiculoCaracteristica != null)
        {
            _context.VehiculoCaracteristicas.Remove(vehiculoCaracteristica);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}


