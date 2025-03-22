using System.Security.Claims;

namespace RentalCars.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUsuarioId(this ClaimsPrincipal usuario)
    {
        var usuarioId = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return usuarioId != null ? Guid.Parse(usuarioId) : Guid.Empty;
    }
}