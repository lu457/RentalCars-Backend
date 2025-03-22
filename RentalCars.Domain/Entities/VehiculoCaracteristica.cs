namespace RentalCars.Domain.Entities;
public record VehiculoCaracteristica
{
    public Guid VehiculoId { get; init; }
    public Guid CaracteristicaId { get; init; }

    public virtual Vehiculo Vehiculo { get; init; } = null!;
    public virtual Caracteristica Caracteristica { get; init; } = null!;
}

