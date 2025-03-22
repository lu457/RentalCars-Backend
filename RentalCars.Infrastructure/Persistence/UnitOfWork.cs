using RentalCars.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace RentalCars.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _currentTransaction;
    private int _transactionCount;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
            _transactionCount = 0;
        }
        _transactionCount++;
    }

    public async Task CommitAsync()
    {
        try
        {
            _transactionCount--;

            if (_transactionCount == 0)
            {
                await _context.SaveChangesAsync();
                await _currentTransaction?.CommitAsync()!;
                await _currentTransaction.DisposeAsync();
            }
        }
        catch
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}



