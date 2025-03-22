using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.HasKey(r => r.Id);

            // Configurar propiedades
            builder.Property(r => r.FechaInicio)
                .IsRequired();

            builder.Property(r => r.FechaFin)
                .IsRequired();

            builder.Property(r => r.PrecioTotal)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(r => r.Comentario)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(r => r.Estado)
                .IsRequired();

            builder.Property(r => r.Calificacion)
                .IsRequired(false);

            // Relación con Vehiculo (Uno a Muchos)
            builder.HasOne(r => r.Vehiculo)
                .WithMany(v => v.Reservas)
                .HasForeignKey(r => r.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Usuario (Uno a Muchos)
            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Sancion (Uno a Uno Opcional)
            builder.HasOne(r => r.Sancion)
                .WithOne()
                .HasForeignKey<Reserva>(r => r.SancionId)
                .IsRequired(false);
        }
    }
}

