using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RentalCars.Domain.Entities;
using RentalCars.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RentalCars.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Definición de las tablas (DbSets)
    public DbSet<Caracteristica> Caracteristicas => Set<Caracteristica>();
    public DbSet<ImagenVehiculo> ImagenVehiculos => Set<ImagenVehiculo>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Regla> Reglas => Set<Regla>();
    public DbSet<Resena> Resenas => Set<Resena>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Sancion> Sanciones => Set<Sancion>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<VehiculoCaracteristica> VehiculoCaracteristicas => Set<VehiculoCaracteristica>();
    public DbSet<VehiculoFavorito> VehiculoFavoritos => Set<VehiculoFavorito>();
    public DbSet<VehiculoRegla> VehiculoReglas => Set<VehiculoRegla>();

    // Configuración de las relaciones y conversiones
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    }
    // Manejo de transacciones
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}