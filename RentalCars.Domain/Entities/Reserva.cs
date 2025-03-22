namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;

public record Reserva : BaseEntity
{
    public DateTime FechaInicio { get; init; }
    public DateTime FechaFin { get; init; }
    public decimal PrecioTotal { get; init; }
    public string Comentario { get; init; } = null!;
    public EstadoReserva Estado { get; init; }
    public int? Calificacion { get; init; }
    public Guid VehiculoId { get; init; }
    public virtual Vehiculo Vehiculo { get; init; } = null!;
    public Guid UsuarioId { get; init; }
    public virtual Usuario Usuario { get; init; } = null!;
    public Guid? SancionId { get; init; }
    public virtual Sancion? Sancion { get; init; }

    public virtual Resena Resena { get; init; } = null!;
}
