using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class ResenaConfiguration : IEntityTypeConfiguration<Resena>
    {
        public void Configure(EntityTypeBuilder<Resena> builder)
        {
            builder.HasKey(rn => rn.Id);

            builder.Property(rn => rn.Comentario)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(rn => rn.Calificacion)
                .IsRequired();

            builder.Property(rn => rn.FechaResena)
                .IsRequired();

            // Relación con Vehiculo (Uno a Muchos)
            builder.HasOne(rn => rn.Vehiculo)
                .WithMany(v => v.Resenas)
                .HasForeignKey(rn => rn.VehiculoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Usuario (Uno a Muchos)
            builder.HasOne(rn => rn.Critico)
                .WithMany(u => u.Resenas)
                .HasForeignKey(rn => rn.CriticoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Reserva (Uno a Uno Opcional)
            builder.HasOne(rn => rn.Reserva)
                .WithOne(r => r.Resena)
                .HasForeignKey<Resena>(rn => rn.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


