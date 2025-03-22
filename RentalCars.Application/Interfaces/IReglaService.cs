using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Reglas;

namespace RentalCars.Application.Interfaces;

public interface IReglaService
{
    Task<Result<IEnumerable<ReglaDto>>> GetAllReglasAsync(CancellationToken cancellationToken = default);
    Task<Result<ReglaDto>> GetReglaByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ReglaDto>> CreateReglaAsync(CreateReglaDto createReglaDto, CancellationToken cancellationToken = default);
}

