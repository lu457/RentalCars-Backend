using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCars.Domain.Enums;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class SancionConfiguration : IEntityTypeConfiguration<Sancion>
    {
        public void Configure(EntityTypeBuilder<Sancion> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Motivo)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(s => s.Monto)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(s => s.FechaSancion)
                .IsRequired();

            builder.Property(s => s.Estado)
                .IsRequired();


            builder.HasOne(s => s.Reserva)
                .WithOne(r => r.Sancion)
                .HasForeignKey<Sancion>(s => s.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

