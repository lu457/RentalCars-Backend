using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Resenas;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;

namespace RentalCars.Application.Services
{
    public class ResenaService : IResenaService
    {
        private readonly IResenaRepository _resenaRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ResenaService(
            IResenaRepository resenaRepository,
             IReservaRepository reservaRepository,
            IVehiculoRepository vehiculoRepository,
            IUnitOfWork unitOfWork)
        {
            _resenaRepository = resenaRepository;
            _reservaRepository = reservaRepository;
            _vehiculoRepository = vehiculoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ResenaResponseDto>>> GetVehiculoResenasAsync(
            Guid vehiculoId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var vehiculo = await _vehiculoRepository.GetByIdAsync(vehiculoId, cancellationToken);
                if (vehiculo == null)
                    return Result<List<ResenaResponseDto>>.Failure("Vehículo no encontrado");

                var resenas = await _resenaRepository.GetByVehiculoIdAsync(vehiculoId, cancellationToken);
                var resenaDtos = resenas.Select(MapToDto).ToList();

                return Result<List<ResenaResponseDto>>.Success(resenaDtos);
            }
            catch (Exception ex)
            {
                return Result<List<ResenaResponseDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<ResenaResponseDto>> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var resena = await _resenaRepository.GetByIdAsync(id, cancellationToken);
                if (resena == null)
                    return Result<ResenaResponseDto>.Failure("Reseña no encontrada");

                return Result<ResenaResponseDto>.Success(MapToDto(resena));
            }
            catch (Exception ex)
            {
                return Result<ResenaResponseDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<ResenaResponseDto>> CreateAsync(
            CreateResenaRequestDto request,
            Guid criticoId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validar que la reserva existe
                var reserva = await _reservaRepository.GetByIdAsync(request.ReservaId, cancellationToken);
                if (reserva == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<ResenaResponseDto>.Failure("Reserva no encontrada");
                }

                // Verificar que el usuario que hace la reseña sea el que realizó la reserva
                if (reserva.UsuarioId != criticoId)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<ResenaResponseDto>.Failure("Solo el usuario que hizo la reserva puede dejar una reseña");
                }

                // Verificar si ya existe una reseña para esta reserva
                var existingResena = await _resenaRepository.GetByReservaIdAsync(request.ReservaId, cancellationToken);
                if (existingResena != null)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<ResenaResponseDto>.Failure("Ya has dejado una reseña para esta reserva");
                }

                // Crear la reseña
                var resena = new Resena
                {
                    Comentario = request.Comentario,
                    Calificacion = request.Calificacion,
                    VehiculoId = request.VehiculoId,
                    CriticoId = criticoId,  // El ID del crítico (usuario que dejó la reseña)
                    ReservaId = request.ReservaId // El ID de la reserva
                };

                await _resenaRepository.AddAsync(resena, cancellationToken);

                // TODO: Actualizar el promedio de calificación del vehículo si es necesario.

                await _unitOfWork.CommitAsync();

                return Result<ResenaResponseDto>.Success(MapToDto(resena));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ResenaResponseDto>.Failure(ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteAsync(
            Guid id,
            Guid criticoId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var resena = await _resenaRepository.GetByIdAsync(id, cancellationToken);

                if (resena == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("Reseña no encontrada");
                }
                if (resena.CriticoId != criticoId)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("No está autorizado para eliminar esta reseña");
                }

                await _resenaRepository.DeleteAsync(resena, cancellationToken);

                await _unitOfWork.CommitAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(ex.Message);
            }
        }

        private static ResenaResponseDto MapToDto(Resena resena)
        {
            return new ResenaResponseDto
            {
                Id = resena.Id,
                Comentario = resena.Comentario,
                Calificacion = resena.Calificacion,
                FechaResena = resena.FechaResena,
                CriticoNombre = $"{resena.Critico.Nombre} {resena.Critico.Apellido}",
                VehiculoId = resena.VehiculoId
            };
        }
    }
}

