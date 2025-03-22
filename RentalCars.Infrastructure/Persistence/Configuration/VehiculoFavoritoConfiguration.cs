using RentalCars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentalCars.Infrastructure.Persistence.Configuration
{
    public class VehiculoFavoritoConfiguration : IEntityTypeConfiguration<VehiculoFavorito>
    {
        public void Configure(EntityTypeBuilder<VehiculoFavorito> builder)
        {
            // Configura la clave primaria compuesta
            builder.HasKey(vf => new { vf.UsuarioId, vf.VehiculoId });

            // Relación con Usuario (Un usuario tiene muchos favoritos)
            builder.HasOne(vf => vf.Usuario)
                .WithMany(u => u.VehiculoFavoritos)
                .HasForeignKey(vf => vf.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Vehiculo (Un vehículo puede ser favorito de muchos usuarios)
            builder.HasOne(vf => vf.Vehiculo)
                .WithMany(v => v.VehiculosFavoritos)
                .HasForeignKey(vf => vf.VehiculoId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}


