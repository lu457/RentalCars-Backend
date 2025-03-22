using RentalCars.Domain.Common;
namespace RentalCars.Domain.Entities;

    public record Caracteristica : BaseEntity
    {
     public string Nombre { get; init; } = null!;
    public virtual ICollection<VehiculoCaracteristica> VehiculoCaracteristicas { get; init; } = [];
}


