using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Nombre)
                .HasMaxLength(100);

            builder.Property(u => u.Apellido)
                .HasMaxLength(100);

            builder.Property(u => u.ContraseñaHash)
                .IsRequired();

            builder.Property(u => u.Celular)
                .IsRequired()
                .HasMaxLength(20);


            // Relación con Vehiculos (Uno a Muchos)
            builder.HasMany(u => u.Vehiculos)
                .WithOne(v => v.Propietario)
                .HasForeignKey(v => v.PropietarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Reservas (Uno a Muchos)
            builder.HasMany(u => u.Reservas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con Reseñas (Uno a Muchos)
            builder.HasMany(u => u.Resenas)
                .WithOne(rn => rn.Critico)
                .HasForeignKey(rn => rn.CriticoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con VehiculosFavoritos (Uno a Muchos)
            builder.HasMany(u => u.VehiculoFavoritos)
                .WithOne(vf => vf.Usuario)
                .HasForeignKey(vf => vf.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Notificaciones (Uno a Muchos)
            builder.HasMany(u => u.Notificaciones)
                .WithOne(n => n.Usuario)
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
