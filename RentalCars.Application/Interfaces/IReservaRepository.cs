using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces;

    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetReservasByVehiculoIdAsync(Guid vehiculoId);
        Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(Guid usuarioId);
        Task<bool> IsVehiculoDisponibleAsync(Guid vehiculoId, DateTime fechaInicio, DateTime fechaFin);
    }


