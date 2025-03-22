using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces
{
    public interface IVehiculoFavoritoRepository : IRepository<VehiculoFavorito>
    {
        Task<bool> EsFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken);
        Task AgregarFavoritoAsync(VehiculoFavorito vehiculoFavorito, CancellationToken cancellationToken);
        Task RemoverFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken);
    }
}
