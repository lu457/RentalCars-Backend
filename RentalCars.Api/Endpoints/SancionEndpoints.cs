using System.Security.Claims;
using RentalCars.Application.DTOs.Sanciones;
using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class SancionEndpoints
{
    public static IEndpointRouteBuilder MapSancionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sanciones")
                      .WithTags("Sanciones")
                      .RequireAuthorization();

        //Obtener todas las sanciones activas
        group.MapGet("/", async (
            [FromServices] ISancionService sancionService,
            CancellationToken cancellationToken) =>
        {
            var result = await sancionService.GetSancionesActivasAsync(cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetSancionesActivas")
        .WithOpenApi()
        .WithSummary("Obtener sanciones activas")
        .WithDescription("Recupera todas las sanciones pendientes en el sistema");

        //Obtener una sanción por ID
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] ISancionService sancionService,
            CancellationToken cancellationToken) =>
        {
            var result = await sancionService.GetByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetSancion")
        .WithOpenApi()
        .WithSummary("Obtener una sanción por ID")
        .WithDescription("Recupera una sanción específica por su ID");

        //Crear una sanción
        group.MapPost("/", async (
            [FromBody] CreateSancionRequestDto request,
            [FromServices] ISancionService sancionService,
            IValidator<CreateSancionRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var result = await sancionService.CreateAsync(request, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/sanciones/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CreateSancion")
        .WithOpenApi()
        .WithSummary("Registrar una nueva sanción")
        .WithDescription("Crea una sanción para una reserva específica");

        //Actualizar el estado de una sanción
        group.MapPut("/{id}/estado", async (
            Guid id,
            [FromBody] UpdateSancionRequestDto request,
            [FromServices] ISancionService sancionService,
            IValidator<UpdateSancionRequestDto> validator,
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

            var result = await sancionService.UpdateStatusAsync(request, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("UpdateSancionEstado")
        .WithOpenApi()
        .WithSummary("Actualizar estado de una sanción")
        .WithDescription("Modifica el estado de una sanción existente");

        //Eliminar una sanción
        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] ISancionService sancionService,
            CancellationToken cancellationToken) =>
        {
            var result = await sancionService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("DeleteSancion")
        .WithOpenApi()
        .WithSummary("Eliminar una sanción")
        .WithDescription("Elimina una sanción del sistema");

        return app;
    }
}
