using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Notificaciones;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Services;

public class NotificacionService : INotificacionService
{
    private readonly INotificacionRepository _notificacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificacionService(INotificacionRepository notificacionRepository, IUnitOfWork unitOfWork)
    {
        _notificacionRepository = notificacionRepository;
        _unitOfWork = unitOfWork;
    }

    /// Crea una notificación cuando se genera un pago o reserva.
    public async Task<Result<NotificacionResponseDto>> CrearNotificacionAsync(
        Guid usuarioId, string mensaje, CancellationToken cancellationToken)
    {
        try
        {
            var notificacion = new Notificacion
            {
                UsuarioId = usuarioId,
                Mensaje = mensaje,
                Estado = EstadoNotificacion.NoLeido,
                FechaCreacion = DateTime.UtcNow
            };

            await _notificacionRepository.AddAsync(notificacion, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<NotificacionResponseDto>.Success(MapToDto(notificacion));
        }
        catch (Exception ex)
        {
            return Result<NotificacionResponseDto>.Failure($"Error al crear la notificación: {ex.Message}");
        }
    }

    /// Obtiene todas las notificaciones de un usuario.
    public async Task<Result<List<NotificacionResponseDto>>> GetByUsuarioIdAsync(
        Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notificaciones = await _notificacionRepository.GetByUsuarioIdAsync(usuarioId, cancellationToken);
            return Result<List<NotificacionResponseDto>>.Success(notificaciones.Select(MapToDto).ToList());
        }
        catch (Exception ex)
        {
            return Result<List<NotificacionResponseDto>>.Failure(ex.Message);
        }
    }

    /// Obtiene las notificaciones no leídas de un usuario.
    public async Task<Result<List<NotificacionResponseDto>>> GetUnreadByUsuarioIdAsync(
        Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notificaciones = await _notificacionRepository.GetUnreadByUsuarioIdAsync(usuarioId, cancellationToken);
            return Result<List<NotificacionResponseDto>>.Success(notificaciones.Select(MapToDto).ToList());
        }
        catch (Exception ex)
        {
            return Result<List<NotificacionResponseDto>>.Failure(ex.Message);
        }
    }

    /// Marca una notificación como leída.
    public async Task<Result<bool>> MarkAsReadAsync(Guid notificacionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var notificacion = await _notificacionRepository.GetByIdAsync(notificacionId, cancellationToken);
            if (notificacion == null)
                return Result<bool>.Failure("Notificación no encontrada");

            await _notificacionRepository.MarkAsReadAsync(notificacionId, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    /// Mapea una entidad Notificacion a un DTO de respuesta.
    private static NotificacionResponseDto MapToDto(Notificacion notificacion)
    {
        return new NotificacionResponseDto
        {
            Id = notificacion.Id,
            Mensaje = notificacion.Mensaje,
            Estado = notificacion.Estado.ToString(),
            FechaCreacion = notificacion.FechaCreacion
        };
    }
}



