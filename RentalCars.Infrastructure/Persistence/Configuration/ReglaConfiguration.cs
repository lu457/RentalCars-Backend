using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCars.Domain.Entities;

namespace RentalCars.Infrastructure.Persistence.Configuration;

public class ReglaConfiguration : IEntityTypeConfiguration<Regla>
{
    public void Configure(EntityTypeBuilder<Regla> builder)
    {
        builder.HasKey(rg => rg.Id);

        builder.Property(rg => rg.Nombre)
            .IsRequired()
            .HasMaxLength(100);
    }
}
