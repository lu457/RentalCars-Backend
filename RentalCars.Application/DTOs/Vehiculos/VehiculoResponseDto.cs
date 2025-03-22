namespace RentalCars.Application.DTOs.Vehiculos;

    public record VehiculoResponseDto
    {
        public Guid Id { get; init; }  // ID del vehículo
        public string Marca { get; init; } = string.Empty;  // Marca del vehículo
        public string Modelo { get; init; } = string.Empty;  // Modelo del vehículo
        public int Year { get; init; }  // Año del vehículo
        public decimal PrecioPorDia { get; init; }  // Precio por día
        public string Ubicacion { get; init; } = string.Empty;  // Ubicación del vehículo
        public string Tipo { get; init; } = string.Empty;  // Tipo de vehículo (por ejemplo, SUV, Sedan)
        public string Estado { get; init; } = string.Empty;  // Estado de disponibilidad del vehículo (Disponible, No Disponible)
        public string Descripcion { get; init; } = string.Empty;  // Descripción del vehículo
        public string Motor { get; init; } = string.Empty;  // Tipo de motor
        public int Cilindros { get; init; }  // Número de cilindros
        public int Puertas { get; init; }  // Número de puertas
        public int CapacidadPasajeros { get; init; }  // Capacidad de pasajeros
        public string Combustible { get; init; } = string.Empty;  // Tipo de combustible (Gasolina, Eléctrico, etc.)
        public string Transmision { get; init; } = string.Empty;  // Tipo de transmisión (Manual, Automática)
        public Guid PropietarioId { get; init; }  // ID del propietario del vehículo
        public List<string> ImageUrls { get; init; } = [];
}


