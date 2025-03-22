using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
    {
        public void Configure(EntityTypeBuilder<Vehiculo> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Modelo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Year)
                .IsRequired();

            builder.Property(v => v.PrecioPorDia)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(v => v.Ubicacion)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Tipo)
            .IsRequired();

            builder.Property(v => v.Estado)
            .IsRequired();

            builder.Property(v => v.Descripcion)
                .HasMaxLength(300);

            builder.Property(v => v.Motor)
                .HasMaxLength(50);

            builder.Property(v => v.Cilindros)
                .IsRequired();

            builder.Property(v => v.Puertas)
                .IsRequired();

            builder.Property(v => v.CapacidadPasajeros)
                .IsRequired();

            builder.Property(v => v.Combustible)
                .IsRequired();

            builder.Property(v => v.Transmision)
                .IsRequired();

            builder.HasOne(v => v.Propietario)
                .WithMany(u => u.Vehiculos)
                .HasForeignKey(v => v.PropietarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.VehiculoCaracteristicas)
                .WithOne(vc => vc.Vehiculo)
                .HasForeignKey(vc => vc.VehiculoId);

            builder.HasMany(p => p.Images)
                .WithOne(i => i.Vehiculo)
                .HasForeignKey(i => i.VehiculoId);

            builder.HasMany(v => v.VehiculoReglas)
                .WithOne(vg => vg.Vehiculo)
                .HasForeignKey(vg => vg.VehiculoId);
        }
    }
}
