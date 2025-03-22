namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;

public record ImagenVehiculo : BaseEntity
{
    public string Url { get; init; } = null!;
    public bool EsPrincipal { get; init; }
    public Guid VehiculoId { get; init; }
    public virtual Vehiculo Vehiculo { get; init; } = null!;
}
