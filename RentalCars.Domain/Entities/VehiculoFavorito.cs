namespace RentalCars.Domain.Entities;

public record VehiculoFavorito
{
    public Guid UsuarioId { get; init; }
    public Guid VehiculoId { get; init; }
    public virtual Usuario Usuario { get; init; } = null!;
    public virtual Vehiculo Vehiculo { get; init; } = null!;
}
