using RentalCars.Application.DTOs.Usuarios;

namespace RentalCars.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioListResponseDto> GetAllUsuariosAsync();
}

