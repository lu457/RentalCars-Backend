namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;

public record Usuario : BaseEntity
{
    public string Email { get; init; } = null!;
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string ContraseñaHash { get; init; } = null!;
    public string Celular { get; init; } = null!;
    public DateTime? UltimaFechaDeIngreso { get; init; } = null!;
    public virtual ICollection<Vehiculo> Vehiculos { get; init; } = [];
    public virtual ICollection<Reserva> Reservas { get; init; } = [];
    public virtual ICollection<Resena> Resenas { get; init; } = [];
    public virtual ICollection<VehiculoFavorito> VehiculoFavoritos { get; init; } = [];
    public virtual ICollection<Notificacion> Notificaciones { get; init; } = [];
}

