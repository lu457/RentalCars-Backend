using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RentalCars.Domain.Enums;

namespace RentalCars.Infrastructure.Repositories;

public class VehiculoRepository : BaseRepository<Vehiculo>, IVehiculoRepository
{
    public VehiculoRepository(ApplicationDbContext context) : base(context)
    {
    }
    public override async Task<IReadOnlyList<Vehiculo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(v => v.Images)
            .ToListAsync(cancellationToken);
    }
    // Método que obtiene los vehículos disponibles en un rango de fechas.
    public async Task<IEnumerable<Vehiculo>> GetAvailableVehiculosAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        return await _dbSet
                .Where(v => v.Estado == Domain.Enums.EstadoDisponibilidad.Disponible)
                .Where(v => !v.Reservas.Any(r =>
                    startDate >= r.FechaInicio && startDate <= r.FechaFin ||
                    endDate >= r.FechaInicio && endDate <= r.FechaFin
                ))
                .Include(v => v.Propietario)
                .Include(v => v.Resenas)
                .Include(v => v.Images)
                .ToListAsync();
    }

    // Método para obtener un vehículo por su ID, con detalles relacionados.
    public override async Task<Vehiculo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
           .AsNoTracking()
            .Include(v => v.Propietario)
            .Include(v => v.Resenas)
            .ThenInclude(r => r.Critico)
            .Include(v => v.Reservas)
            .Include(v => v.Images)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    // Método que permite filtrar vehículos según varios parámetros (ubicación, precio, características).
    public async Task<IEnumerable<Vehiculo>> GetVehiculosByFiltersAsync(
        string? ubicacion = null,
        TipoDeVehiculo? tipoVehiculo = null
    )
    {
        var query = _dbSet.Include(v => v.Images).AsQueryable();

        if (!string.IsNullOrEmpty(ubicacion))
            query = query.Where(v => v.Ubicacion.Contains(ubicacion));

        if (tipoVehiculo.HasValue)
            query = query.Where(v => v.Tipo == tipoVehiculo.Value);

        return await query.ToListAsync();
    }

    // Método para obtener vehículos por propietario
    public async Task<IEnumerable<Vehiculo>> GetVehiculosByPropietarioAsync(Guid propietarioId)
    {
        return await _dbSet
            .Where(v => v.PropietarioId == propietarioId) // Filtra por propietario
            .Include(v => v.Images)
            .Include(v => v.Resenas)
            .Include(v => v.Reservas)
            .ToListAsync();
    }


}
