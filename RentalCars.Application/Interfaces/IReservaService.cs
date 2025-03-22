using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Reservas;

namespace RentalCars.Application.Interfaces;

    public interface IReservaService
    {
        Task<Result<List<ReservaResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Result<List<ReservaResponseDto>>> GetUserReservasAsync(Guid usuarioId, CancellationToken cancellationToken = default);

        Task<Result<ReservaDetailDto>> GetByIdAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);

        Task<Result<ReservaResponseDto>> CreateAsync(CreateReservaRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default);

        Task<Result<ReservaResponseDto>> UpdateStatusAsync(UpdateReservaRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default);

        Task<Result<bool>> CancelReservaAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);
    }


