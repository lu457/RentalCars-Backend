namespace RentalCars.Domain.Entities;

using RentalCars.Domain.Common;

public record Resena : BaseEntity
{
    public string Comentario { get; init; } = null!;
    public int Calificacion { get; init; }
    public DateTime FechaResena { get; init; } = DateTime.UtcNow;  // Fecha en que se dejó la reseña
    public Guid VehiculoId { get; init; }
    public virtual Vehiculo Vehiculo { get; init; } = null!;
    public Guid CriticoId { get; init; }
    public virtual Usuario Critico { get; init; } = null!;
    public Guid ReservaId { get; init; }
    public virtual Reserva Reserva { get; init; } = null!;
}
