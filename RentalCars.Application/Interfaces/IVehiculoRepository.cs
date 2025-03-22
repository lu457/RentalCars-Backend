using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces
{
    public interface IVehiculoRepository : IRepository<Vehiculo>
    {
        Task<IEnumerable<Vehiculo>> GetAvailableVehiculosAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<Vehiculo>> GetVehiculosByFiltersAsync(string? ubicacion = null, decimal? minPrecio = null, decimal? maxPrecio = null, List<Guid>? caracteristicaIds = null);
    }
}