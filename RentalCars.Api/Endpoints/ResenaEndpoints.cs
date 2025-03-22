using System.Security.Claims;
using RentalCars.Application.DTOs.Resenas;
using RentalCars.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class ResenaEndpoints
{
    public static IEndpointRouteBuilder MapResenaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenas")
                      .WithTags("Reseñas");

        // GET: api/resenas/vehiculo/{vehiculoId}
        group.MapGet("/vehiculo/{vehiculoId}", async (
            Guid vehiculoId,
            [FromServices] IResenaService resenaService,
            CancellationToken cancellationToken) =>
        {
            var result = await resenaService.GetVehiculoResenasAsync(vehiculoId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetResenasVehiculo")
        .WithOpenApi()
        .WithSummary("Obtener reseñas de un vehículo")
        .WithDescription("Recupera todas las reseñas asociadas a un vehículo específico");

        // GET: api/resenas/{id}
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] IResenaService resenaService,
            CancellationToken cancellationToken) =>
        {
            var result = await resenaService.GetByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetResena")
        .WithOpenApi()
        .WithSummary("Obtener una reseña por ID")
        .WithDescription("Recupera una reseña específica por su ID");

        // POST: api/resenas
        group.MapPost("/", async (
            [FromBody] CreateResenaRequestDto request,
            [FromServices] IResenaService resenaService,
            IValidator<CreateResenaRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var usuarioId = user.GetUsuarioId();
            var result = await resenaService.CreateAsync(request, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/resenas/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("CreateResena")
        .WithOpenApi()
        .WithSummary("Crear una nueva reseña")
        .WithDescription("Crea una nueva reseña para un vehículo");

        // DELETE: api/resenas/{id}
        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] IResenaService resenaService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await resenaService.DeleteAsync(id, usuarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new { message = "Reseña eliminada exitosamente" })
                : Results.NotFound(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("DeleteResena")
        .WithOpenApi()
        .WithSummary("Eliminar una reseña")
        .WithDescription("Elimina una reseña existente");

        return app;
    }
}
