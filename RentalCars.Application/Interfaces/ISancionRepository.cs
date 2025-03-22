using RentalCars.Domain.Entities;

namespace RentalCars.Application.Interfaces
{
    public interface ISancionRepository : IRepository<Sancion>
    {
        Task<Sancion?> GetByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Sancion>> GetSancionesActivasAsync(CancellationToken cancellationToken = default);
    }
}


