using RentalCars.Domain.Enums;

namespace RentalCars.Application.DTOs.Vehiculos;

    public record UpdateVehiculoRequestDto : CreateVehiculoRequestDto
    {
       public EstadoDisponibilidad Estado;  // Estado del vehículo (Disponible, No Disponible)

       public Guid Id { get; init; }  // ID del vehículo que se va a actualizar
    }


