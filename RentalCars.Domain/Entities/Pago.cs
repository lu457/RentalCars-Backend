namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;
using RentalCars.Domain.Enums;

public record Pago : BaseEntity
{
    public Guid ReservaId { get; init; }
    public decimal Monto { get; init; }
    public MetodoPago MetodoPago{ get; init; }
    public EstadoTransaccion Estado { get; init; }

    public virtual Reserva Reserva { get; init; } = null!;
}
