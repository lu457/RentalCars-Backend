using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCars.Domain.Entities;

namespace RentalCars.Infrastructure.Persistence.Configuration;

public class VehiculoReglaConfiguration : IEntityTypeConfiguration<VehiculoRegla>
{
    public void Configure(EntityTypeBuilder<VehiculoRegla> builder)
    {
        builder.HasKey(vr => new { vr.VehiculoId, vr.ReglaId }); // Clave compuesta

        builder.HasOne(vr => vr.Vehiculo)
            .WithMany(v => v.VehiculoReglas)
            .HasForeignKey(vr => vr.VehiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vr => vr.Regla)
            .WithMany(r => r.VehiculoReglas)
            .HasForeignKey(vr => vr.ReglaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
