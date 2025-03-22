using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class ImagenVehiculoConfiguration : IEntityTypeConfiguration<ImagenVehiculo>
    {
        public void Configure(EntityTypeBuilder<ImagenVehiculo> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Url)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.EsPrincipal)
                .IsRequired();
        }
    }
}

