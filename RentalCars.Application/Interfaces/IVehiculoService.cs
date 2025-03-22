using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Vehiculos;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Interfaces
{
    public interface IVehiculoService
    {
        Task<Result<List<VehiculoResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<VehiculoDetailDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<VehiculoResponseDto>> CreateAsync(CreateVehiculoRequestDto request, Guid propietarioId, CancellationToken cancellationToken = default);
        Task<Result<VehiculoResponseDto>> UpdateAsync(UpdateVehiculoRequestDto request, Guid propietarioId, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, Guid propietarioId, CancellationToken cancellationToken = default);
        Task<Result<bool>> CheckOwnershipAsync(Guid propietarioId, Guid usuarioId, CancellationToken cancellationToken = default);

        // Métodos para manejar favoritos
        Task<Result<bool>> AgregarFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<Result<bool>> RemoverFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<Result<bool>> EsFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default);

        // Método para filtrar vehículos
        Task<Result<List<VehiculoResponseDto>>> FiltrarVehiculosAsync(
            string? ubicacion = null,
            TipoDeVehiculo? tipoVehiculo = null,
            CancellationToken cancellationToken = default);
    }
}
