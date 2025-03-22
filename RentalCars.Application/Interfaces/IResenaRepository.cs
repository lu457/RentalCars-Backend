using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces
{
    public interface IResenaRepository : IRepository<Resena>
    {
        Task<IEnumerable<Resena>> GetResenasByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<double> GetAverageRatingByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Resena>> GetByVehiculoIdAsync(Guid vehiculoId, CancellationToken cancellationToken = default);
        Task<Resena?> GetByVehiculoAndCriticoAsync(Guid vehiculoId, Guid criticoId, CancellationToken cancellationToken = default);
        Task<Resena?> GetByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default);
    }
}


