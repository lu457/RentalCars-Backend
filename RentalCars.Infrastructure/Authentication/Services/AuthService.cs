namespace RentalCars.Infrastructure.Authentication.Services;

using RentalCars.Application.Common;
using RentalCars.Application.DTOs.Auth;
using RentalCars.Application.Interfaces;
using RentalCars.Domain.Entities;
using RentalCars.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly JwtConfig _jwtConfig;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor de la clase AuthService, inyecta las dependencias necesarias
    public AuthService(
        IOptions<JwtConfig> jwtConfig,
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _jwtConfig = jwtConfig.Value;
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    // Método para iniciar sesión
    public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Comienza la transacción
            await _unitOfWork.BeginTransactionAsync();

            // Obtener el usuario por correo electrónico
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email, cancellationToken);

            // Verifica si el usuario existe y si la contraseña ingresada es correcta
            if (usuario == null ||
                _passwordHasher.VerifyHashedPassword(usuario, usuario.ContraseñaHash, request.Contraseña)
                == PasswordVerificationResult.Failed)
            {
                return Result<AuthResponseDto>.Failure("Credenciales inválidas");
            }

            // Actualizar la última fecha de inicio de sesión
            var ultimaFechaDeIngreso = await _usuarioRepository.UpdateLastLoginAsync(usuario.Id, cancellationToken);
            if (!ultimaFechaDeIngreso)
            {
                // Si no se puede actualizar la fecha de último inicio de sesión, revertir la transacción
                await _unitOfWork.RollbackAsync();
                return Result<AuthResponseDto>.Failure("Error actualizando la última fecha de inicio de sesión");
            }

            // Confirmar la transacción
            await _unitOfWork.CommitAsync();

            // Generar el token JWT y devolver la respuesta con el token y la información del usuario
            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = await GenerateTokenAsync(usuario, cancellationToken),
                Email = usuario.Email,
                NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}"
            });
        }
        catch (Exception ex)
        {
            // Si ocurre algún error, revertir la transacción
            await _unitOfWork.RollbackAsync();
            return Result<AuthResponseDto>.Failure(ex.Message);
        }
    }


    // Método para registrar un nuevo usuario
    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingUsuario = await _usuarioRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUsuario != null)
            {
                await _unitOfWork.RollbackAsync();
                return Result<AuthResponseDto>.Failure("El correo ya está registrado");
            }

            // Crea una nueva instancia de usuario y encripta la contraseña
            var usuario = new Usuario
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Celular = request.Celular,
                ContraseñaHash = _passwordHasher.HashPassword(null!, request.Contraseña)
            };

            await _usuarioRepository.AddAsync(usuario, cancellationToken);
            await _unitOfWork.CommitAsync();

            // Devuelve el token y la información del usuario recién registrado
            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = await GenerateTokenAsync(usuario, cancellationToken),
                Email = usuario.Email,
                NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}"
            });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<AuthResponseDto>.Failure(ex.Message);
        }
    }

    // Método para generar un token JWT con los datos del usuario
    public async Task<string> GenerateTokenAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Método para validar un token JWT recibido
    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
