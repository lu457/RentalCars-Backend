namespace RentalCars.Domain.Entities;

public record VehiculoRegla
{
    public Guid VehiculoId { get; init; }
    public Guid ReglaId { get; init; }

    public virtual Vehiculo Vehiculo { get; init; } = null!;
    public virtual Regla Regla { get; init; } = null!;
}
