using RentalCars.Application.DTOs.Pagos;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Application.Common;
using RentalCars.Domain.Enums;
using RentalCars.Application.DTOs.Reservas;

namespace RentalCars.Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PagoService(
            IPagoRepository pagoRepository,
            IReservaRepository reservaRepository,
            IUnitOfWork unitOfWork)
        {
            _pagoRepository = pagoRepository;
            _reservaRepository = reservaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PagoResponseDto>>> GetUserPagosAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pagos = await _pagoRepository.GetUserPagosAsync(usuarioId, cancellationToken);
                return Result<List<PagoResponseDto>>.Success(pagos.Select(MapToDto).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<PagoResponseDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<PagoResponseDto>>> GetPagosByPropietarioAsync(Guid propietarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pagos = await _pagoRepository.GetPagosByPropietarioAsync(propietarioId, cancellationToken);
                return Result<List<PagoResponseDto>>.Success(pagos.Select(MapToDto).ToList());
            }
            catch (Exception ex)
            {
                return Result<List<PagoResponseDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagoDetailDto>> GetByIdAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pago = await _pagoRepository.GetByIdAsync(id, cancellationToken);
                if (pago == null)
                    return Result<PagoDetailDto>.Failure("Pago no encontrado");

                if (pago.Reserva.UsuarioId != usuarioId && pago.Reserva.Vehiculo.PropietarioId != usuarioId)
                    return Result<PagoDetailDto>.Failure("No tienes permiso para ver este pago");

                return Result<PagoDetailDto>.Success(MapToDetailDto(pago));
            }
            catch (Exception ex)
            {
                return Result<PagoDetailDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagoResponseDto>> CreateAsync(CreatePagoRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var reserva = await _reservaRepository.GetByIdAsync(request.ReservaId, cancellationToken);
                if (reserva == null)
                    return Result<PagoResponseDto>.Failure("Reserva no encontrada");

                if (reserva.UsuarioId != usuarioId)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<PagoResponseDto>.Failure("Solo el dueño de la reserva puede realizar el pago");
                }

                var existePago = await _pagoRepository.ExistePagoParaReservaAsync(request.ReservaId, cancellationToken);
                if (existePago)
                    return Result<PagoResponseDto>.Failure("Ya existe un pago registrado para esta reserva");

                var pago = new Pago
                {
                    ReservaId = request.ReservaId,
                    Monto = request.Monto,
                    MetodoPago = Enum.Parse<MetodoPago>(request.MetodoPago),
                    Estado = EstadoTransaccion.Pendiente
                };

                await _pagoRepository.AddAsync(pago, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Result<PagoResponseDto>.Success(MapToDto(pago));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<PagoResponseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagoResponseDto>> UpdateStatusAsync(UpdatePagoRequestDto request, Guid usuarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var pago = await _pagoRepository.GetByIdAsync(request.Id, cancellationToken);
                if (pago == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<PagoResponseDto>.Failure("Pago no encontrado");
                }

                if (pago.Reserva.Vehiculo.PropietarioId != usuarioId)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<PagoResponseDto>.Failure("Solo el propietario del vehículo puede actualizar el estado del pago");
                }

                var updatedPago = pago with { Estado = Enum.Parse<EstadoTransaccion>(request.Estado) };

                await _pagoRepository.UpdateAsync(updatedPago);
                await _unitOfWork.CommitAsync();

                return Result<PagoResponseDto>.Success(MapToDto(updatedPago));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<PagoResponseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> CancelarPagoAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var pago = await _pagoRepository.GetByIdAsync(id, cancellationToken);
                if (pago == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("Pago no encontrado");
                }

                // Solo el usuario que hizo el pago o el propietario del vehículo pueden cancelarlo
                if (pago.Reserva.UsuarioId != usuarioId && pago.Reserva.Vehiculo.PropietarioId != usuarioId)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("No tienes permiso para cancelar este pago");
                }

                // Validación: Solo se pueden cancelar pagos en estado Pendiente
                if (pago.Estado != EstadoTransaccion.Pendiente)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("Solo se pueden cancelar pagos pendientes");
                }

                // Cambiar estado a Cancelado
                var updatedPago = pago with { Estado = EstadoTransaccion.Cancelado };

                await _pagoRepository.UpdateAsync(updatedPago, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(ex.Message);
            }
        }

        private static PagoResponseDto MapToDto(Pago pago)
        {
            return new PagoResponseDto
            {
                Id = pago.Id,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago.ToString(),
                Estado = pago.Estado.ToString(),
                ReservaId = pago.ReservaId
            };
        }

        private static PagoDetailDto MapToDetailDto(Pago pago)
        {
            return new PagoDetailDto
            {
                Id = pago.Id,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago.ToString(),
                Estado = pago.Estado.ToString(),
                Reserva = new ReservaResponseDto
                {
                    Id = pago.Reserva.Id,
                    FechaInicio = pago.Reserva.FechaInicio,
                    FechaFin = pago.Reserva.FechaFin,
                    PrecioTotal = pago.Reserva.PrecioTotal,
                    Estado = pago.Reserva.Estado.ToString()
                }
            };
        }
    }
}



