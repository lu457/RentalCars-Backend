using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Reglas;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;

namespace RentalCars.Application.Services;

public class ReglaService : IReglaService
{
    private readonly IReglaRepository _reglaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReglaService(
        IReglaRepository reglaRepository,
        IUnitOfWork unitOfWork)
    {
        _reglaRepository = reglaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<ReglaDto>>> GetAllReglasAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var reglas = await _reglaRepository.GetAllAsync(cancellationToken);
            return Result<IEnumerable<ReglaDto>>.Success(
                reglas.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ReglaDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<ReglaDto>> GetReglaByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var regla = await _reglaRepository.GetByIdAsync(id, cancellationToken);
            if (regla == null)
                return Result<ReglaDto>.Failure("Regla no encontrada");

            return Result<ReglaDto>.Success(MapToDto(regla));
        }
        catch (Exception ex)
        {
            return Result<ReglaDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ReglaDto>> CreateReglaAsync(
        CreateReglaDto createReglaDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingRegla = await _reglaRepository.GetByNameAsync(createReglaDto.Nombre, cancellationToken);
            if (existingRegla != null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ReglaDto>.Failure("Ya existe una regla con este nombre");
            }

            var regla = new Regla
            {
                Nombre = createReglaDto.Nombre
            };

            await _reglaRepository.AddAsync(regla, cancellationToken);
            await _unitOfWork.CommitAsync();

            return Result<ReglaDto>.Success(MapToDto(regla));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<ReglaDto>.Failure(ex.Message);
        }
    }

    private static ReglaDto MapToDto(Regla regla)
    {
        return new ReglaDto(
            Id: regla.Id,
            Nombre: regla.Nombre
        );
    }
}
