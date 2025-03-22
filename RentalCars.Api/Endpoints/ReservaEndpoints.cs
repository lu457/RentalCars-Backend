using System.Security.Claims;
using RentalCars.Application.DTOs.Reservas;
using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class ReservaEndpoints
{
    public static IEndpointRouteBuilder MapReservaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservas")
                      .WithTags("Reservas")
                      .RequireAuthorization();

        // GET: api/reservas
        group.MapGet("/", async (
            [FromServices] IReservaService reservaService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await reservaService.GetUserReservasAsync(usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetUserReservas")
        .WithOpenApi()
        .WithSummary("Obtener reservas del usuario")
        .WithDescription("Recupera todas las reservas del usuario actual");

        // GET: api/reservas/{id}
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] IReservaService reservaService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await reservaService.GetByIdAsync(id, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetReserva")
        .WithOpenApi()
        .WithSummary("Obtener una reserva por ID")
        .WithDescription("Recupera una reserva específica por su ID");

        // POST: api/reservas
        group.MapPost("/", async (
            [FromBody] CreateReservaRequestDto request,
            [FromServices] IReservaService reservaService,
            IValidator<CreateReservaRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var usuarioId = user.GetUsuarioId();
            var result = await reservaService.CreateAsync(request, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/reservas/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CreateReserva")
        .WithOpenApi()
        .WithSummary("Crear una nueva reserva")
        .WithDescription("Crea una nueva reserva para un vehículo");

        // PUT: api/reservas/{id}/estado
        group.MapPut("/{id}/estado", async (
            Guid id,
            [FromBody] UpdateReservaRequestDto request,
            [FromServices] IReservaService reservaService,
            IValidator<UpdateReservaRequestDto> validator,
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
            var result = await reservaService.UpdateStatusAsync(request, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("UpdateReservaEstado")
        .WithOpenApi()
        .WithSummary("Actualizar estado de una reserva")
        .WithDescription("Actualiza el estado de una reserva existente");

        // DELETE: api/reservas/{id}
        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] IReservaService reservaService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await reservaService.CancelReservaAsync(id, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("CancelReserva")
        .WithOpenApi()
        .WithSummary("Cancelar una reserva")
        .WithDescription("Cancela una reserva existente");

        return app;
    }
}
