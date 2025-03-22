using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces;

public interface ICaracteristicaRepository : IRepository<Caracteristica>
{
    // Obtener una característica por su nombre
    Task<Caracteristica?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default);

    // Verificar si una característica ya está asociada a un vehículo
    Task<bool> ExistsVehiculoCaracteristicaAsync(Guid vehiculoId, Guid caracteristicaId, CancellationToken cancellationToken = default);

    // Listar caracteristicas de un vehiculo
    Task<IEnumerable<Caracteristica>> GetByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default);

    // Agregar una característica a un vehículo
    Task AddVehiculoCaracteristicaAsync(Guid vehiculoId, Guid caracteristicaId, CancellationToken cancellationToken = default);

    // Eliminar una característica de un vehículo
    Task RemoveVehiculoCaracteristicaAsync(Guid vehiculoId, Guid caracteristicaId, CancellationToken cancellationToken = default);

}

