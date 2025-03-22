namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;

public record Regla : BaseEntity
{
    public string Nombre { get; init; } = string.Empty;
    public virtual ICollection<VehiculoRegla> VehiculoReglas { get; init; } = [];
}
