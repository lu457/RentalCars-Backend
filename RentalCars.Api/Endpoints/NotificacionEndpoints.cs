using System.Security.Claims;
using RentalCars.Application.DTOs.Notificaciones;
using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class NotificacionEndpoints
{
    public static IEndpointRouteBuilder MapNotificacionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notificaciones")
                      .WithTags("Notificaciones")
                      .RequireAuthorization();

        //Obtener todas las notificaciones del usuario actual
        group.MapGet("/", async (
            [FromServices] INotificacionService notificacionService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await notificacionService.GetByUsuarioIdAsync(usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetUserNotificaciones")
        .WithOpenApi()
        .WithSummary("Obtener todas las notificaciones del usuario")
        .WithDescription("Recupera todas las notificaciones del usuario actual, tanto leídas como no leídas.");

        //Obtener solo las notificaciones NO LEÍDAS
        group.MapGet("/noleidas", async (
            [FromServices] INotificacionService notificacionService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await notificacionService.GetUnreadByUsuarioIdAsync(usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetUnreadNotificaciones")
        .WithOpenApi()
        .WithSummary("Obtener notificaciones no leídas")
        .WithDescription("Recupera solo las notificaciones no leídas del usuario actual.");

        //Marcar una notificación como leída
        group.MapPut("/{id}/leer", async (
            Guid id,
            [FromServices] INotificacionService notificacionService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await notificacionService.MarkAsReadAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("MarkNotificacionAsRead")
        .WithOpenApi()
        .WithSummary("Marcar una notificación como leída")
        .WithDescription("Cambia el estado de una notificación específica a 'Leído'.");

        return app;
    }
}
