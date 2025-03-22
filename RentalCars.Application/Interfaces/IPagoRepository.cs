using RentalCars.Domain.Entities;


namespace RentalCars.Application.Interfaces;

public interface IPagoRepository : IRepository<Pago>
{
    /// Obtiene todos los pagos asociados a una reserva específica.
    Task<IEnumerable<Pago>> GetPagosByReservaIdAsync(Guid reservaId, CancellationToken cancellationToken = default);

    /// Verifica si una reserva ya tiene un pago registrado.
    Task<bool> ExistePagoParaReservaAsync(Guid reservaId, CancellationToken cancellationToken = default);

    /// Obtiene todos los pagos realizados por un usuario.
    Task<IEnumerable<Pago>> GetUserPagosAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    /// Obtiene todos los pagos de las reservas de un propietario de vehículo.
    Task<IEnumerable<Pago>> GetPagosByPropietarioAsync(Guid propietarioId, CancellationToken cancellationToken = default);
}

