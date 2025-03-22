
namespace RentalCars.Application.DTOs.Usuarios;

public record UsuarioListResponseDto(IEnumerable<UsuarioDto> Usuarios);
public record UsuarioDto(
    Guid Id,
    string Email,
    string Nombre,
    string Apellido,
    string Celular,
    DateTime? UltimaFechaDeIngreso
);
