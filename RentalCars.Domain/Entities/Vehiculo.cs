using RentalCars.Domain.Common;
using RentalCars.Domain.Enums;
using RentalCars.Domain.ValueObjects;
namespace RentalCars.Domain.Entities;

public record Vehiculo : BaseEntity
{
    public string Marca { get; init; } = string.Empty;
    public string Modelo { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal PrecioPorDia { get; init; }
    public string Ubicacion { get; init; } = string.Empty;
    public TipoDeVehiculo Tipo { get; init; }
    public EstadoDisponibilidad Estado { get; init; }
    public string Descripcion { get; init; } = string.Empty;
    public string Motor { get; init; } = string.Empty;
    public int Cilindros { get; init; }
    public int Puertas { get; init; }
    public int CapacidadPasajeros { get; init; }
    public TipoCombustible Combustible { get; init; }
    public TipoTransmision Transmision { get; init; }
    public Guid PropietarioId { get; init; }
    public virtual Usuario Propietario { get; init; } = null!;
    public virtual ICollection<VehiculoCaracteristica> VehiculoCaracteristicas { get; init; } = [];
    public virtual ICollection<VehiculoRegla> VehiculoReglas { get; init; } = [];
    public virtual ICollection<Reserva> Reservas { get; init; } = [];
    public virtual ICollection<Resena> Resenas { get; init; } = [];
    public virtual ICollection<ImagenVehiculo> Images { get; init; } = [];
    public virtual ICollection<VehiculoFavorito> VehiculosFavoritos { get; init; } = [];



    protected Vehiculo() { }

    public Vehiculo(string marca, string modelo, int year, Dinero precioPorDia, Direccion ubicacion, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(marca))
            throw new ArgumentException("La marca no puede estar vacía.");
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("El modelo no puede estar vacío.");
        if (year <= 0)
            throw new ArgumentException("El año debe ser un valor positivo.");
        if (precioPorDia.Monto <= 0)
            throw new ArgumentException("El precio por día debe ser mayor a cero.");

        Marca = marca;
        Modelo = modelo;
        Year = year;
        PrecioPorDia = precioPorDia.Monto;
        Ubicacion = $"{ubicacion.Calle}, {ubicacion.Ciudad}, {ubicacion.Pais}";
        Descripcion = descripcion;
    }
}


