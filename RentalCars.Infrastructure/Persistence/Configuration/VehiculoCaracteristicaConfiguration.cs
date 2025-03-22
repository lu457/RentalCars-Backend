using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCars.Domain.Entities;

namespace RentalCars.Infrastructure.Persistence.Configuration;

public class VehiculoCaracteristicaConfiguration : IEntityTypeConfiguration<VehiculoCaracteristica>
{
    public void Configure(EntityTypeBuilder<VehiculoCaracteristica> builder)
    {
        builder.HasKey(vc => new { vc.VehiculoId, vc.CaracteristicaId });

        builder.HasOne(vc => vc.Vehiculo)
            .WithMany(v => v.VehiculoCaracteristicas)
            .HasForeignKey(vc => vc.VehiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Caracteristica (Muchos a Uno)
        builder.HasOne(vc => vc.Caracteristica)
            .WithMany(c => c.VehiculoCaracteristicas)
            .HasForeignKey(vc => vc.CaracteristicaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
