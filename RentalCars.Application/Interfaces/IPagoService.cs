using RentalCars.Application.DTOs.Pagos;
using RentalCars.Application.Common;

namespace RentalCars.Application.Interfaces
{
    public interface IPagoService
    {
        Task<Result<List<PagoResponseDto>>> GetUserPagosAsync(Guid usuarioId, CancellationToken cancellationToken = default);
        Task<Result<List<PagoResponseDto>>> GetPagosByPropietarioAsync(Guid propietarioId, CancellationToken cancellationToken = default);
        Task<Result<PagoDetailDto>> GetByIdAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);
        Task<Result<PagoResponseDto>> CreateAsync(CreatePagoRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default);
        Task<Result<PagoResponseDto>> UpdateStatusAsync(UpdatePagoRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default);
        Task<Result<bool>> CancelarPagoAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);
    }
}

