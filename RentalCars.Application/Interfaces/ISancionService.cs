using RentalCars.Application.DTOs.Sanciones;
using RentalCars.Application.Common;

namespace RentalCars.Application.Interfaces
{
    public interface ISancionService
    {
        Task<Result<List<SancionResponseDto>>> GetSancionesActivasAsync(CancellationToken cancellationToken = default);
        Task<Result<SancionDetailDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<SancionResponseDto>> CreateAsync(CreateSancionRequestDto request, CancellationToken cancellationToken = default);
        Task<Result<SancionResponseDto>> UpdateStatusAsync(UpdateSancionRequestDto request, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
