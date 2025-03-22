using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Caracteristicas;

namespace RentalCars.Application.Interfaces;

public interface ICaracteristicaService
{
    // Obtiene todas las características
    Task<Result<IEnumerable<CaracteristicaDto>>> GetAllCaracteristicasAsync(CancellationToken cancellationToken = default);

    // Obtiene una característica por su ID
    Task<Result<CaracteristicaDto>> GetCaracteristicaByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Crea una nueva característica
    Task<Result<CaracteristicaDto>> CreateCaracteristicaAsync(CreateCaracteristicaDto createCaracteristicaDto, CancellationToken cancellationToken = default);

    // Listar caracteristicas de un vehiculo
    Task<Result<IEnumerable<CaracteristicaDto>>> GetCaracteristicasByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default);

    // Agregar una característica a un vehículo
    Task<Result<bool>> AddVehiculoCaracteristicaAsync(Guid vehiculoId, Guid caracteristicaId, CancellationToken cancellationToken = default);

    // Eliminar una característica de un vehículo
    Task<Result<bool>> RemoveVehiculoCaracteristicaAsync(Guid vehiculoId, Guid caracteristicaId, CancellationToken cancellationToken = default);

}
