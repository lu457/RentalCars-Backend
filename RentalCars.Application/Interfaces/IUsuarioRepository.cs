using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UpdateLastLoginAsync(Guid usuarioId, CancellationToken cancellationToken = default);
}
