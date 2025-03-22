using RentalCars.Application.DTOs.Vehiculos;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Application.DTOs.Resenas;

namespace RentalCars.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificacionService _notificacionService;

    public ReservaService(
        IReservaRepository reservaRepository,
        IVehiculoRepository vehiculoRepository,
        IUnitOfWork unitOfWork,
        INotificacionService notificacionService)
    {
        _reservaRepository = reservaRepository;
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
        _notificacionService = notificacionService;
    }

    public async Task<Result<List<ReservaResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var reservas = await _reservaRepository.GetAllAsync(cancellationToken);
            return Result<List<ReservaResponseDto>>.Success(
                reservas.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<List<ReservaResponseDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<List<ReservaResponseDto>>> GetUserReservasAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var reservas = await _reservaRepository.GetReservasByUsuarioIdAsync(usuarioId);
            return Result<List<ReservaResponseDto>>.Success(
                reservas.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<List<ReservaResponseDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<ReservaDetailDto>> GetByIdAsync(
        Guid id,
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var reserva = await _reservaRepository.GetByIdAsync(id, cancellationToken);
            if (reserva == null)
                return Result<ReservaDetailDto>.Failure("Reserva no encontrada");

            if (reserva.UsuarioId != usuarioId && reserva.Vehiculo.PropietarioId != usuarioId)
                return Result<ReservaDetailDto>.Failure("No estás autorizado a ver esta reserva");

            return Result<ReservaDetailDto>.Success(MapToDetailDto(reserva));
        }
        catch (Exception ex)
        {
            return Result<ReservaDetailDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ReservaResponseDto>> CreateAsync(
        CreateReservaRequestDto request,
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var vehiculo = await _vehiculoRepository.GetByIdAsync(request.VehiculoId, cancellationToken);
            if (vehiculo == null)
                return Result<ReservaResponseDto>.Failure("Vehículo no encontrado");

            if (request.FechaInicio >= request.FechaFin)
                return Result<ReservaResponseDto>.Failure("La fecha de inicio debe ser antes de la fecha de fin");

            if (request.FechaInicio < DateTime.Today)
                return Result<ReservaResponseDto>.Failure("La fecha de inicio no puede ser en el pasado");

            var isAvailable = await _reservaRepository.IsVehiculoDisponibleAsync(
                request.VehiculoId,
                request.FechaInicio,
                request.FechaFin
            );

            if (!isAvailable)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ReservaResponseDto>.Failure("El vehículo no está disponible para esas fechas");
            }

            var dias = (request.FechaFin - request.FechaInicio).Days;
            var precioTotal = vehiculo.PrecioPorDia * dias;

            var reserva = new Reserva
            {
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Comentario = request.Comentario,
                PrecioTotal = precioTotal,
                VehiculoId = request.VehiculoId,
                UsuarioId = usuarioId,
                Estado = EstadoReserva.Pendiente
            };

            await _reservaRepository.AddAsync(reserva, cancellationToken);
            await _unitOfWork.CommitAsync();

            var reservaConVehiculo = await _reservaRepository.GetByIdAsync(reserva.Id, cancellationToken);

            // Enviar notificación al propietario del vehículo
            await _notificacionService.CrearNotificacionAsync(
                vehiculo.PropietarioId,
                $"Se ha realizado una nueva reserva para el vehículo {vehiculo.Marca} {vehiculo.Modelo}.",
                cancellationToken
            );

            return Result<ReservaResponseDto>.Success(MapToDto(reserva));

        }
        catch (Exception ex)
        {
            var innerExceptionMessage = ex.InnerException?.Message ?? "Sin detalles adicionales";
            return Result<ReservaResponseDto>.Failure($"Error al guardar los cambios en la base de datos: {innerExceptionMessage}");
        }
    }


    public async Task<Result<ReservaResponseDto>> UpdateStatusAsync(
        UpdateReservaRequestDto request,
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var reserva = await _reservaRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reserva == null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ReservaResponseDto>.Failure("Reserva no encontrada");
            }

            if (reserva.Vehiculo.PropietarioId != usuarioId)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ReservaResponseDto>.Failure("Solo el dueño del vehículo puede actualizar el estado de la reserva");
            }

            var updatedReserva = reserva with { Estado = Enum.Parse<EstadoReserva>(request.Estado) };

            await _reservaRepository.UpdateAsync(updatedReserva);
            await _unitOfWork.CommitAsync();

            // Notificación al usuario sobre la actualización de su reserva
            await _notificacionService.CrearNotificacionAsync(
                reserva.UsuarioId,
                $"Tu reserva ha sido {request.Estado}.",
                cancellationToken
            );

            return Result<ReservaResponseDto>.Success(MapToDto(updatedReserva));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<ReservaResponseDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> CancelReservaAsync(
        Guid id,
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var reserva = await _reservaRepository.GetByIdAsync(id, cancellationToken);
            if (reserva == null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("Reserva no encontrada");
            }

            if (reserva.UsuarioId != usuarioId && reserva.Vehiculo.PropietarioId != usuarioId)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("No tienes permisos para cancelar esta reserva");
            }

            if (reserva.FechaInicio <= DateTime.Now)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("No se puede cancelar una reserva que ya ha comenzado");
            }

            var updatedReserva = reserva with { Estado = EstadoReserva.Cancelada };

            await _reservaRepository.UpdateAsync(updatedReserva, cancellationToken);
            await _unitOfWork.CommitAsync();

            //Notificaciones a ambas partes
            await _notificacionService.CrearNotificacionAsync(
                reserva.UsuarioId,
                "Tu reserva ha sido cancelada.",
                cancellationToken
            );
            await _notificacionService.CrearNotificacionAsync(
                reserva.Vehiculo.PropietarioId,
                "Una reserva de tu vehículo ha sido cancelada.",
                cancellationToken
            );

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<bool>.Failure(ex.Message);
        }
    }



        private static ReservaResponseDto MapToDto(Reserva reserva)
        {
            return new ReservaResponseDto
            {
                Id = reserva.Id,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                Comentario = reserva.Comentario,
                PrecioTotal = reserva.PrecioTotal,
                Estado = reserva.Estado.ToString(),
            };
        }
        private static ReservaDetailDto MapToDetailDto(Reserva reserva)
        {
            var baseDto = MapToDto(reserva);
            var resena = reserva.Resena;

        return new ReservaDetailDto
            {
                Id = baseDto.Id,
                FechaInicio = baseDto.FechaInicio,
                FechaFin = baseDto.FechaFin,
                Comentario= baseDto.Comentario,
                PrecioTotal = baseDto.PrecioTotal,
                Estado = baseDto.Estado,
                Vehiculo = new VehiculoResponseDto
            {
                Id = reserva.Vehiculo.Id,
                Marca = reserva.Vehiculo.Marca,
                Modelo = reserva.Vehiculo.Modelo,
                Year = reserva.Vehiculo.Year,
                PrecioPorDia = reserva.Vehiculo.PrecioPorDia,
                Ubicacion = reserva.Vehiculo.Ubicacion,
                Tipo = reserva.Vehiculo.Tipo.ToString(),
                Estado = reserva.Vehiculo.Estado.ToString(),
                Descripcion = reserva.Vehiculo.Descripcion,
                Motor = reserva.Vehiculo.Motor,
                Cilindros = reserva.Vehiculo.Cilindros,
                Puertas = reserva.Vehiculo.Puertas,
                CapacidadPasajeros = reserva.Vehiculo.CapacidadPasajeros,
                Combustible = reserva.Vehiculo.Combustible.ToString(),
                Transmision = reserva.Vehiculo.Transmision.ToString(),
                PropietarioId = reserva.Vehiculo.PropietarioId,

            },
                // Mapeamos la reseña asociada a la reserva
                Resena = resena != null ? new ResenaResponseDto
                {
                    Id = resena.Id,
                    Calificacion = resena.Calificacion,
                    Comentario = resena.Comentario,
                    FechaResena = resena.FechaResena,
                    CriticoNombre = $"{resena.Critico.Nombre} {resena.Critico.Apellido}",
                    VehiculoId = resena.VehiculoId,
                } : null
            };
        }

    }


