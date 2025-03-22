using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RentalCars.Domain.Entities;

namespace RentalCars.Infrastructure.Persistence.Configuration;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        // Definir la clave primaria
        builder.HasKey(p => p.Id);

        // Configurar propiedades
        builder.Property(p => p.Monto)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.MetodoPago)
            .IsRequired();

        builder.Property(p => p.Estado)
            .IsRequired();

        // Relación con Reserva (Uno a Uno)
        builder.HasOne(p => p.Reserva)
            .WithOne()
            .HasForeignKey<Pago>(p => p.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
