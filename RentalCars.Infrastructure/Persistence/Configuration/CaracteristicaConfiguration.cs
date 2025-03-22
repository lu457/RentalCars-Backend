using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCars.Domain.Entities;

namespace RentalCars.Infrastructure.Persistence.Configuration;

public class CaracteristicaConfiguration : IEntityTypeConfiguration<Caracteristica>
{
    public void Configure(EntityTypeBuilder<Caracteristica> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);
    }
}