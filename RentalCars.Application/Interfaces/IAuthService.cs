namespace RentalCars.Application.Interfaces;

using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Auth;
using RentalCars.Domain.Entities;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    Task<string> GenerateTokenAsync(Usuario usuario, CancellationToken cancellationToken = default);

    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}
