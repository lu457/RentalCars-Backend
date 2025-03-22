using RentalCars.Application.DTOs.Sanciones;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Services
{
    public class SancionService : ISancionService
    {
        private readonly ISancionRepository _sancionRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SancionService(
            ISancionRepository sancionRepository,
            IReservaRepository reservaRepository,
            IUnitOfWork unitOfWork)
        {
            _sancionRepository = sancionRepository;
            _reservaRepository = reservaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<SancionResponseDto>>> GetSancionesActivasAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var sanciones = await _sancionRepository.GetSancionesActivasAsync(cancellationToken);
                return Result<List<SancionResponseDto>>.Success(sanciones.Select(MapToDto).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<SancionResponseDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<SancionDetailDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var sancion = await _sancionRepository.GetByIdAsync(id, cancellationToken);
                if (sancion == null)
                    return Result<SancionDetailDto>.Failure("Sanción no encontrada");

                return Result<SancionDetailDto>.Success(MapToDetailDto(sancion));
            }
            catch (Exception ex)
            {
                return Result<SancionDetailDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<SancionResponseDto>> CreateAsync(CreateSancionRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Verificar si la reserva existe
                var reserva = await _reservaRepository.GetByIdAsync(request.ReservaId, cancellationToken);
                if (reserva == null)
                    return Result<SancionResponseDto>.Failure("Reserva no encontrada");

                var sancion = new Sancion
                {
                    Motivo = request.Motivo,
                    Monto = request.Monto,
                    FechaSancion = DateTime.UtcNow,
                    Estado = EstadoSancion.Pendiente,
                    ReservaId = request.ReservaId
                };

                await _sancionRepository.AddAsync(sancion, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Result<SancionResponseDto>.Success(MapToDto(sancion));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<SancionResponseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<SancionResponseDto>> UpdateStatusAsync(UpdateSancionRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var sancion = await _sancionRepository.GetByIdAsync(request.Id, cancellationToken);
                if (sancion == null)
                    return Result<SancionResponseDto>.Failure("Sanción no encontrada");

                var updatedSancion = sancion with { Estado = Enum.Parse<EstadoSancion>(request.Estado) };

                await _sancionRepository.UpdateAsync(updatedSancion);
                await _unitOfWork.CommitAsync();

                return Result<SancionResponseDto>.Success(MapToDto(updatedSancion));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<SancionResponseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var sancion = await _sancionRepository.GetByIdAsync(id, cancellationToken);
                if (sancion == null)
                    return Result<bool>.Failure("Sanción no encontrada");

                await _sancionRepository.DeleteAsync(sancion, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(ex.Message);
            }
        }

        private static SancionResponseDto MapToDto(Sancion sancion)
        {
            return new SancionResponseDto
            {
                Id = sancion.Id,
                Motivo = sancion.Motivo,
                Monto = sancion.Monto,
                FechaSancion = sancion.FechaSancion,
                Estado = sancion.Estado.ToString(),
                ReservaId = sancion.ReservaId
            };
        }

        private static SancionDetailDto MapToDetailDto(Sancion sancion)
        {
            return new SancionDetailDto
            {
                Id = sancion.Id,
                Motivo = sancion.Motivo,
                Monto = sancion.Monto,
                FechaSancion = sancion.FechaSancion,
                Estado = sancion.Estado.ToString(),
                Reserva = new ReservaResponseDto
                {
                    Id = sancion.Reserva.Id,
                    FechaInicio = sancion.Reserva.FechaInicio,
                    FechaFin = sancion.Reserva.FechaFin,
                    PrecioTotal = sancion.Reserva.PrecioTotal,
                    Estado = sancion.Reserva.Estado.ToString()
                }
            };
        }
    }
}


