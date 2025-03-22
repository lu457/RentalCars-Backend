namespace RentalCars.Application.Interfaces;

    public interface IUnitOfWork : IDisposable
    {
        // Método para guardar los cambios de manera asincrónica
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
