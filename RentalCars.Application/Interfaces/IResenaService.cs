using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Resenas;

    public interface IResenaService
    {
        Task<Result<List<ResenaResponseDto>>> GetVehiculoResenasAsync(Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<Result<ResenaResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<ResenaResponseDto>> CreateAsync(CreateResenaRequestDto request, Guid criticoId, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, Guid criticoId, CancellationToken cancellationToken = default);
    }


