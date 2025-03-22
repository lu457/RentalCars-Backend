using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Caracteristicas;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;

namespace RentalCars.Application.Services;

public class CaracteristicaService : ICaracteristicaService
{
    private readonly ICaracteristicaRepository _caracteristicaRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CaracteristicaService(
        ICaracteristicaRepository caracteristicaRepository,
        IVehiculoRepository vehiculoRepository,
        IUnitOfWork unitOfWork)
    {
        _caracteristicaRepository = caracteristicaRepository;
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
    }

    // Obtiene todas las características
    public async Task<Result<IEnumerable<CaracteristicaDto>>> GetAllCaracteristicasAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var caracteristicas = await _caracteristicaRepository.GetAllAsync(cancellationToken);
            return Result<IEnumerable<CaracteristicaDto>>.Success(
                caracteristicas.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CaracteristicaDto>>.Failure(ex.Message);
        }
    }

    // Obtiene una característica por su ID
    public async Task<Result<CaracteristicaDto>> GetCaracteristicaByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var caracteristica = await _caracteristicaRepository.GetByIdAsync(id, cancellationToken);
            if (caracteristica == null)
                return Result<CaracteristicaDto>.Failure("Característica no encontrada");

            return Result<CaracteristicaDto>.Success(MapToDto(caracteristica));
        }
        catch (Exception ex)
        {
            return Result<CaracteristicaDto>.Failure(ex.Message);
        }
    }

    // Crea una nueva característica
    public async Task<Result<CaracteristicaDto>> CreateCaracteristicaAsync(
        CreateCaracteristicaDto createCaracteristicaDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingCaracteristica = await _caracteristicaRepository.GetByNameAsync(
                createCaracteristicaDto.Nombre,
                cancellationToken
            );

            if (existingCaracteristica != null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<CaracteristicaDto>.Failure("Ya existe una característica con este nombre");
            }

            var caracteristica = new Caracteristica
            {
                Nombre = createCaracteristicaDto.Nombre
            };

            await _caracteristicaRepository.AddAsync(caracteristica, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<CaracteristicaDto>.Success(MapToDto(caracteristica));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<CaracteristicaDto>.Failure(ex.Message);
        }
    }

    // Listar caracteristicas de un vehiculo
    public async Task<Result<IEnumerable<CaracteristicaDto>>> GetCaracteristicasByVehiculoIdAsync(
    Guid vehiculoId,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var caracteristicas = await _caracteristicaRepository.GetByVehiculoIdAsync(vehiculoId, cancellationToken);

            if (!caracteristicas.Any())
                return Result<IEnumerable<CaracteristicaDto>>.Failure("El vehículo no tiene características asignadas.");

            return Result<IEnumerable<CaracteristicaDto>>.Success(
                caracteristicas.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CaracteristicaDto>>.Failure(ex.Message);
        }
    }


    // Agregar una característica a un vehículo
    public async Task<Result<bool>> AddVehiculoCaracteristicaAsync(
        Guid vehiculoId,
        Guid caracteristicaId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var caracteristica = await _caracteristicaRepository.GetByIdAsync(caracteristicaId, cancellationToken);
            if (caracteristica == null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("La característica no existe.");
            }

            var vehiculo = await _vehiculoRepository.GetByIdAsync(vehiculoId, cancellationToken);
            if (vehiculo == null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("El vehículo no existe.");
            }


            var existeRelacion = await _caracteristicaRepository.ExistsVehiculoCaracteristicaAsync(vehiculoId, caracteristicaId, cancellationToken);
            if (existeRelacion)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("La característica ya está asociada a este vehículo.");
            }

            await _caracteristicaRepository.AddVehiculoCaracteristicaAsync(vehiculoId, caracteristicaId, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<bool>.Failure(ex.Message);
        }
    }

    // Eliminar una característica de un vehículo
    public async Task<Result<bool>> RemoveVehiculoCaracteristicaAsync(
        Guid vehiculoId,
        Guid caracteristicaId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _caracteristicaRepository.RemoveVehiculoCaracteristicaAsync(vehiculoId, caracteristicaId, cancellationToken);

            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<bool>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<bool>.Failure("Error al eliminar la característica del vehículo.");
        }
    }


    // Convierte una entidad Caracteristica en un DTO
    private static CaracteristicaDto MapToDto(Caracteristica caracteristica)
    {
        return new CaracteristicaDto(
            Id: caracteristica.Id,
            Nombre: caracteristica.Nombre
        );
    }
}


