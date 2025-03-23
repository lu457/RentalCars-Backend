using RentalCars.Domain.Entities;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Interfaces
{
    public interface IVehiculoRepository : IRepository<Vehiculo>
    {
        Task<IEnumerable<Vehiculo>> GetAvailableVehiculosAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<Vehiculo>> GetVehiculosByFiltersAsync(string? ubicacion = null, TipoDeVehiculo? tipoVehiculo = null);

        Task<IEnumerable<Vehiculo>> GetVehiculosByPropietarioAsync(Guid propietarioId);
    }
}