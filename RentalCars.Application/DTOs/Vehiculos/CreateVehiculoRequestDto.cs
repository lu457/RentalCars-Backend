namespace RentalCars.Application.DTOs.Vehiculos;

    public record CreateVehiculoRequestDto
    {
        public string Marca { get; init; } = string.Empty;  // Marca del vehículo
        public string Modelo { get; init; } = string.Empty;  // Modelo del vehículo
        public int Year { get; init; }  // Año del vehículo
        public decimal PrecioPorDia { get; init; }  // Precio por día del alquiler}
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Tipo { get; init; } = string.Empty;  // Tipo de vehículo (SUV, Sedan, etc.)
        public string Descripcion { get; init; } = string.Empty;  // Descripción del vehículo
        public string Motor { get; init; } = string.Empty;  // Tipo de motor
        public int Cilindros { get; init; }  // Número de cilindros
        public int Puertas { get; init; }  // Número de puertas
        public int CapacidadPasajeros { get; init; }  // Capacidad de pasajeros
        public string Combustible { get; init; } = string.Empty;  // Tipo de combustible (Gasolina, Eléctrico, etc.)
        public string Transmision { get; init; } = string.Empty;  // Tipo de transmisión (Manual, Automática)
    }

