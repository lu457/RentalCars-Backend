using RentalCars.Application.DTOs.Vehiculos;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Resenas;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Domain.ValueObjects;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Services;
        public class VehiculoService : IVehiculoService
{
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IVehiculoFavoritoRepository _vehiculoFavoritoRepository;
        private readonly IUnitOfWork _unitOfWork;

        //Constructor
        public VehiculoService(IVehiculoRepository vehiculoRepository, IVehiculoFavoritoRepository vehiculoFavoritoRepository, IUnitOfWork unitOfWork)
        {
            _vehiculoRepository = vehiculoRepository;
            _vehiculoFavoritoRepository = vehiculoFavoritoRepository;
            _unitOfWork = unitOfWork;
        }

    // Método para agregar un vehículo a favoritos
    public async Task<Result<bool>> AgregarFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existe = await _vehiculoFavoritoRepository.EsFavoritoAsync(usuarioId, vehiculoId, cancellationToken);
            if (existe)
            {
                return Result<bool>.Failure("El vehículo ya está en favoritos.");
            }

            var favorito = new VehiculoFavorito
            {
                UsuarioId = usuarioId,
                VehiculoId = vehiculoId
            };

            await _vehiculoFavoritoRepository.AgregarFavoritoAsync(favorito, cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    // Método para remover un vehículo de favoritos
    public async Task<Result<bool>> RemoverFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existe = await _vehiculoFavoritoRepository.EsFavoritoAsync(usuarioId, vehiculoId, cancellationToken);
            if (!existe)
            {
                return Result<bool>.Failure("El vehículo no está en favoritos.");
            }

            await _vehiculoFavoritoRepository.RemoverFavoritoAsync(usuarioId, vehiculoId, cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    // Método para verificar si un vehículo está en favoritos
    public async Task<Result<bool>> EsFavoritoAsync(Guid usuarioId, Guid vehiculoId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar si el vehículo está en los favoritos
            var esFavorito = await _vehiculoFavoritoRepository.EsFavoritoAsync(usuarioId, vehiculoId, cancellationToken);

            // Si no es favorito, devolver un mensaje claro
            if (!esFavorito)
            {
                return Result<bool>.Failure("Este vehículo no está en tus favoritos.");
            }

            // Si es favorito, devolver true
            return Result<bool>.Success(esFavorito);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }


    // Obtener todos los vehículos
    public async Task<Result<List<VehiculoResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var vehiculos = await _vehiculoRepository.GetAllAsync(cancellationToken);
                var vehiculoDtos = vehiculos.Select(MapToResponseDto).ToList();
                return Result<List<VehiculoResponseDto>>.Success(vehiculoDtos);
            }
            catch (Exception ex)
            {
                return Result<List<VehiculoResponseDto>>.Failure(ex.Message);
            }
        }

    public async Task<Result<bool>> CheckOwnershipAsync(
    Guid propietarioId,
    Guid usuarioId,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(propietarioId, cancellationToken);

            if (vehiculo == null)
                return Result<bool>.Failure("Vehiculo no encontrado");

            return Result<bool>.Success(vehiculo.PropietarioId == usuarioId);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }


    public async Task<Result<List<VehiculoResponseDto>>> FiltrarVehiculosAsync(
    string? ubicacion = null,
    TipoDeVehiculo? tipoVehiculo = null,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var vehiculos = await _vehiculoRepository.GetVehiculosByFiltersAsync(ubicacion, tipoVehiculo);

            var vehiculoDtos = vehiculos.Select(MapToResponseDto).ToList();

            return Result<List<VehiculoResponseDto>>.Success(vehiculoDtos);
        }
        catch (Exception ex)
        {
            return Result<List<VehiculoResponseDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<List<VehiculoResponseDto>>> GetVehiculosByPropietarioAsync(
    Guid propietarioId,
    CancellationToken cancellationToken = default)
    {
        try
        {
            // Llamamos al repositorio para obtener los vehículos del propietario
            var vehiculos = await _vehiculoRepository.GetVehiculosByPropietarioAsync(propietarioId);

            // Convertimos la lista de entidades a DTOs
            var vehiculoDtos = vehiculos.Select(MapToResponseDto).ToList();

            return Result<List<VehiculoResponseDto>>.Success(vehiculoDtos);
        }
        catch (Exception ex)
        {
            return Result<List<VehiculoResponseDto>>.Failure(ex.Message);
        }
    }


    // Obtener detalles de un vehículo por su ID
    public async Task<Result<VehiculoDetailDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var vehiculo = await _vehiculoRepository.GetByIdAsync(id, cancellationToken);
                if (vehiculo == null)
                    return Result<VehiculoDetailDto>.Failure("Vehículo no encontrado");

                var vehiculoDto = MapToDetailDto(vehiculo);
                return Result<VehiculoDetailDto>.Success(vehiculoDto);
            }
            catch (Exception ex)
            {
                return Result<VehiculoDetailDto>.Failure(ex.Message);
            }
        }

        // Crear un nuevo vehículo
        public async Task<Result<VehiculoResponseDto>> CreateAsync(
            CreateVehiculoRequestDto request,
            Guid propietarioId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var ubicacion = new Direccion(request.Calle, request.Ciudad, request.Pais);
                var precio = new Dinero(request.PrecioPorDia);

                var vehiculo = new Vehiculo(
                    request.Marca,
                    request.Modelo,
                    request.Year,
                    precio,
                    ubicacion,
                    request.Descripcion
                )
                {
                    PropietarioId = propietarioId,
                    Tipo = Enum.Parse<Domain.Enums.TipoDeVehiculo>(request.Tipo),
                    Estado = Domain.Enums.EstadoDisponibilidad.Disponible,
                    Motor = request.Motor,
                    Cilindros = request.Cilindros,
                    Puertas = request.Puertas,
                    CapacidadPasajeros = request.CapacidadPasajeros,
                    Combustible = Enum.Parse<Domain.Enums.TipoCombustible>(request.Combustible),
                    Transmision = Enum.Parse<Domain.Enums.TipoTransmision>(request.Transmision)
                };

                await _vehiculoRepository.AddAsync(vehiculo, cancellationToken);
                await _unitOfWork.CommitAsync();

                return Result<VehiculoResponseDto>.Success(MapToResponseDto(vehiculo));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<VehiculoResponseDto>.Failure(ex.Message);
            }
        }

    // Actualizar un vehículo
    public async Task<Result<VehiculoResponseDto>> UpdateAsync(
        UpdateVehiculoRequestDto request,
        Guid propietarioId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // Obtener el vehículo actual
            var vehiculo = await _vehiculoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vehiculo == null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<VehiculoResponseDto>.Failure("Vehículo no encontrado");
            }

            if (vehiculo.PropietarioId != propietarioId)
            {
                await _unitOfWork.RollbackAsync();
                return Result<VehiculoResponseDto>.Failure("No estás autorizado a actualizar este vehículo");
            }

            // Crear una nueva instancia de Vehiculo (record) con los valores actualizados
            var updatedVehiculo = vehiculo with
            {
                Marca = request.Marca,
                Modelo = request.Modelo,
                Year = request.Year,
                PrecioPorDia = request.PrecioPorDia,
                Ubicacion = $"{request.Calle}, {request.Ciudad}, {request.Pais}",
                Descripcion = request.Descripcion,
                Tipo = Enum.Parse<Domain.Enums.TipoDeVehiculo>(request.Tipo),
                Estado = request.Estado, // Aquí se actualiza el estado
                Motor = request.Motor,
                Cilindros = request.Cilindros,
                Puertas = request.Puertas,
                CapacidadPasajeros = request.CapacidadPasajeros,
                Combustible = Enum.Parse<Domain.Enums.TipoCombustible>(request.Combustible),
                Transmision = Enum.Parse<Domain.Enums.TipoTransmision>(request.Transmision),
                FechaActualizacion = DateTime.Now // Aquí se actualiza la fecha de actualización
            };

            await _vehiculoRepository.UpdateAsync(updatedVehiculo, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<VehiculoResponseDto>.Success(MapToResponseDto(updatedVehiculo));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<VehiculoResponseDto>.Failure(ex.Message);
        }
    }


    // Eliminar un vehículo
    public async Task<Result<bool>> DeleteAsync(
            Guid id,
            Guid propietarioId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var vehiculo = await _vehiculoRepository.GetByIdAsync(id, cancellationToken);
                if (vehiculo == null)
                    return Result<bool>.Failure("Vehículo no encontrado");

                if (vehiculo.PropietarioId != propietarioId)
                    return Result<bool>.Failure("No estás autorizado a eliminar este vehículo");

                await _vehiculoRepository.DeleteAsync(vehiculo, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
            return Result<bool>.Failure($"Error al eliminar el vehículo: {ex.Message}");
        }
        }
        private static VehiculoResponseDto MapToResponseDto(Vehiculo vehiculo)
        {
            return new VehiculoResponseDto
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Year = vehiculo.Year,
                PrecioPorDia = vehiculo.PrecioPorDia,
                Ubicacion = vehiculo.Ubicacion,
                Tipo = vehiculo.Tipo.ToString(),
                Estado = vehiculo.Estado.ToString(),
                Descripcion = vehiculo.Descripcion,
                Motor = vehiculo.Motor,
                Cilindros = vehiculo.Cilindros,
                Puertas = vehiculo.Puertas,
                CapacidadPasajeros = vehiculo.CapacidadPasajeros,
                Combustible = vehiculo.Combustible.ToString(),
                Transmision = vehiculo.Transmision.ToString(),
                PropietarioId = vehiculo.PropietarioId,
                ImageUrls = vehiculo.Images.Select(img => $"https://localhost:7297{img.Url}").ToList()
            };
        }

    private static VehiculoDetailDto MapToDetailDto(Vehiculo vehiculo)
    {
        return new VehiculoDetailDto
        {
            // Propiedades básicas del vehículo
            Id = vehiculo.Id,
            Marca = vehiculo.Marca,
            Modelo = vehiculo.Modelo,
            Year = vehiculo.Year,
            PrecioPorDia = vehiculo.PrecioPorDia,
            Ubicacion = vehiculo.Ubicacion,
            Tipo = vehiculo.Tipo.ToString(),
            Estado = vehiculo.Estado.ToString(),
            Descripcion = vehiculo.Descripcion,
            Motor = vehiculo.Motor,
            Cilindros = vehiculo.Cilindros,
            Puertas = vehiculo.Puertas,
            CapacidadPasajeros = vehiculo.CapacidadPasajeros,
            Combustible = vehiculo.Combustible.ToString(),
            Transmision = vehiculo.Transmision.ToString(),
            PropietarioId = vehiculo.PropietarioId,

            // Información adicional del propietario
            PropietarioNombre = vehiculo.Propietario.Nombre,

            // Calificación promedio de las reseñas del vehículo
            CalificacionPromedio = vehiculo.Resenas.Any()
                ? vehiculo.Resenas.Average(rn => rn.Calificacion)  // Calcula el promedio de las calificaciones
                : 0,  // Si no tiene reseñas, asigna 0

            // Reseñas asociadas al vehículo
            Resenas = vehiculo.Resenas.Select(rn => new ResenaResponseDto
            {
                Id = rn.Id,
                Calificacion = rn.Calificacion,
                FechaResena = rn.FechaResena,
                Comentario = rn.Comentario,
                CriticoNombre = $"{rn.Critico.Nombre} {rn.Critico.Apellido}",
                VehiculoId = rn.VehiculoId
            }).ToList(),

            // Reservas asociadas al vehículo
            Reservas = vehiculo.Reservas.Select(r => new ReservaResponseDto
            {
                Id = r.Id,
                FechaInicio = r.FechaInicio,
                FechaFin = r.FechaFin,
                PrecioTotal = r.PrecioTotal,
                Estado = r.Estado.ToString(),
                Vehiculo = MapToResponseDto(r.Vehiculo)  // Mapea el vehículo de la reserva
            }).ToList(),
            ImageUrls = vehiculo.Images.Select(img => $"https://localhost:7297{img.Url}").ToList()
        };
    }
}

