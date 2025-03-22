using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    //Obtiene una entidad por su ID
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    //Obtiene todas las entidades de este tipo
    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    //Agrega una nueva entidad a la base de datos
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    //Actualiza una entidad de la base de datos

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        // _dbSet.Update(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }



    //Elimina una entidad de la base de datos
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    //Obtiene una lista paginada de entidades, junto con el total de registros
    public virtual async Task<(IReadOnlyList<T> Items, int Total)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var total = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
