namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;
using RentalCars.Domain.Enums;
public record Sancion : BaseEntity
{
    public string Motivo { get; init; } = string.Empty;
    public decimal Monto { get; init; }
    public DateTime FechaSancion { get; init; } = DateTime.UtcNow;
    public EstadoSancion Estado { get; init; }
    public Guid ReservaId { get; init; }
    public virtual Reserva Reserva { get; init; } = null!;
}
