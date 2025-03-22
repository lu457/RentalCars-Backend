namespace RentalCars.Domain.Common;

public abstract record BaseEntity
{
    public Guid Id { get; init; }
    public DateTime FechaCreacion { get; init; }
    public DateTime? FechaActualizacion { get; init; }

    // Constructor para inicializar las propiedades
    protected BaseEntity()
    {
        Id = Guid.NewGuid(); // Genera un GUID único para cada entidad
        FechaCreacion = DateTime.Now; // Se asigna la fecha y hora de creación
    }
}
