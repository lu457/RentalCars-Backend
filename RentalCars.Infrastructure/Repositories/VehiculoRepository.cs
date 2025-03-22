using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
          // .AsNoTracking()
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
        decimal? minPrecio = null,
        decimal? maxPrecio = null,
        List<Guid>? caracteristicaIds = null
    )
    {
        var query = _dbSet.AsQueryable();

        // Aplica filtro de ubicación si se proporciona.
        if (!string.IsNullOrEmpty(ubicacion))
            query = query.Where(v => v.Ubicacion.Contains(ubicacion));

        // Aplica filtro de precio mínimo si se proporciona.
        if (minPrecio.HasValue)
            query = query.Where(v => v.PrecioPorDia >= minPrecio.Value);

        // Aplica filtro de precio máximo si se proporciona.
        if (maxPrecio.HasValue)
            query = query.Where(v => v.PrecioPorDia <= maxPrecio.Value);

        // Aplica filtro de características si se proporcionan.
        if (caracteristicaIds?.Any() == true)
            query = query.Where(v => v.VehiculoCaracteristicas
                .Any(vc => caracteristicaIds.Contains(vc.CaracteristicaId)));

        return await query
            // .AsNoTracking()
            .Include(v => v.Ubicacion)
            .Include(v => v.VehiculoCaracteristicas)
            .ToListAsync();
    }
}
