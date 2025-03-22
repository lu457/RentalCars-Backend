using System.Security.Claims;
using RentalCars.Application.DTOs.Pagos;
using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class PagoEndpoints
{
    public static IEndpointRouteBuilder MapPagoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/pagos")
                      .WithTags("Pagos")
                      .RequireAuthorization();

        // Obtener los pagos del usuario autenticado
        group.MapGet("/", async (
            [FromServices] IPagoService pagoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await pagoService.GetUserPagosAsync(usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetUserPagos")
        .WithOpenApi()
        .WithSummary("Obtener pagos del usuario")
        .WithDescription("Recupera todos los pagos realizados por el usuario autenticado.");

        //Obtener pagos de reservas de un propietario
        group.MapGet("/propietario", async (
            [FromServices] IPagoService pagoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var propietarioId = user.GetUsuarioId();
            var result = await pagoService.GetPagosByPropietarioAsync(propietarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetPagosByPropietario")
        .WithOpenApi()
        .WithSummary("Obtener pagos de reservas de un propietario")
        .WithDescription("Obtiene todos los pagos de reservas asociadas a los vehículos del propietario autenticado.");

        // 📌 Obtener un pago por ID
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] IPagoService pagoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await pagoService.GetByIdAsync(id, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetPagoById")
        .WithOpenApi()
        .WithSummary("Obtener un pago por ID")
        .WithDescription("Recupera un pago específico por su ID, validando permisos.");

        //Registrar un nuevo pago
        group.MapPost("/", async (
            [FromBody] CreatePagoRequestDto request,
            [FromServices] IPagoService pagoService,
            IValidator<CreatePagoRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var usuarioId = user.GetUsuarioId();
            var result = await pagoService.CreateAsync(request, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/pagos/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CreatePago")
        .WithOpenApi()
        .WithSummary("Registrar un nuevo pago")
        .WithDescription("Registra un pago en el sistema para una reserva.");

        //Actualizar el estado de un pago (Aprobar/Rechazar)
        group.MapPut("/{id}/estado", async (
            Guid id,
            [FromBody] UpdatePagoRequestDto request,
            [FromServices] IPagoService pagoService,
            IValidator<UpdatePagoRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest(new { error = "El ID no coincide con la solicitud" });
            }

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var usuarioId = user.GetUsuarioId();
            var result = await pagoService.UpdateStatusAsync(request, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("UpdatePagoEstado")
        .WithOpenApi()
        .WithSummary("Actualizar estado de un pago")
        .WithDescription("Permite al propietario aprobar o rechazar un pago.");

        //Cancelar un pago (Solo si está en estado Pendiente)
        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] IPagoService pagoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await pagoService.CancelarPagoAsync(id, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CancelPago")
        .WithOpenApi()
        .WithSummary("Cancelar un pago")
        .WithDescription("Permite al usuario cancelar un pago si aún está en estado Pendiente.");

        return app;
    }
}



