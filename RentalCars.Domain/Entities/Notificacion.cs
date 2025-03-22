namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;
using RentalCars.Domain.Enums;

public record Notificacion : BaseEntity
{
    public Guid UsuarioId { get; init; }
    public string Mensaje { get; init; } = string.Empty;
    public EstadoNotificacion Estado { get; set; }
    public virtual Usuario Usuario { get; init; } = null!;
}
