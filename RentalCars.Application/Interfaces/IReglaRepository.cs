using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces;

public interface IReglaRepository : IRepository<Regla>
{
    Task<Regla?> GetByNameAsync(string nombre, CancellationToken cancellationToken);
    Task AddVehiculoReglaAsync(Guid vehiculoId, Guid reglaId, CancellationToken cancellationToken);
}

