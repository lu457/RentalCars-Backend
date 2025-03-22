using RentalCars.Application.DTOs.Usuarios;
using RentalCars.Application.Interfaces;

namespace RentalCars.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    // Obtener todos los usuarios
    public async Task<UsuarioListResponseDto> GetAllUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        var usuarioDtos = usuarios.Select(u => new UsuarioDto(
            u.Id,
            u.Email,
            u.Nombre,
            u.Apellido,
            u.Celular,
            u.UltimaFechaDeIngreso
        ));
        return new UsuarioListResponseDto(usuarioDtos);
    }
}
