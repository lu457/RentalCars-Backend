using RentalCars.Application.DTOs.Caracteristicas;
using RentalCars.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class CaracteristicaEndpoints
{
    public static IEndpointRouteBuilder MapCaracteristicaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/caracteristicas")
                      .WithTags("Características");

        // GET: api/caracteristicas
        group.MapGet("/", async (
            [FromServices] ICaracteristicaService caracteristicaService,
            CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.GetAllCaracteristicasAsync(cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetAllCaracteristicas")
        .WithOpenApi()
        .WithSummary("Obtener todas las características")
        .WithDescription("Recupera todas las características disponibles en el sistema");



        // GET: api/caracteristicas/{id}
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] ICaracteristicaService caracteristicaService,
            CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.GetCaracteristicaByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetCaracteristicaById")
        .WithOpenApi()
        .WithSummary("Obtener una característica por ID")
        .WithDescription("Recupera una característica específica por su ID");



        // POST: api/caracteristicas
        group.MapPost("/", async (
            CreateCaracteristicaDto createCaracteristicaDto,
            [FromServices] ICaracteristicaService caracteristicaService,
            CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.CreateCaracteristicaAsync(createCaracteristicaDto, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/caracteristicas/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CreateCaracteristica")
        .WithOpenApi()
        .WithSummary("Crear una nueva característica")
        .WithDescription("Crea una nueva característica con los detalles especificados");



        // GET: api/caracteristicas/vehiculo/{vehiculoId}
        group.MapGet("/vehiculo/{vehiculoId}/caracteristicas", async (
        Guid vehiculoId,
        [FromServices] ICaracteristicaService caracteristicaService,
        CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.GetCaracteristicasByVehiculoIdAsync(vehiculoId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetCaracteristicasByVehiculoId")
        .WithOpenApi()
        .WithSummary("Obtener características de un vehículo")
        .WithDescription("Lista todas las características asociadas a un vehículo por su ID");



        // POST: api/caracteristicas/{vehiculoId}/add/{caracteristicaId}
        group.MapPost("/{vehiculoId}/add/{caracteristicaId}", async (
            Guid vehiculoId,
            Guid caracteristicaId,
            [FromServices] ICaracteristicaService caracteristicaService,
            CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.AddVehiculoCaracteristicaAsync(vehiculoId, caracteristicaId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new { message = "Característica agregada exitosamente al vehículo." }) // ✅ Mensaje de éxito
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("AddVehiculoCaracteristica")
        .WithOpenApi()
        .WithSummary("Agregar una característica a un vehículo")
        .WithDescription("Asocia una característica específica a un vehículo por sus respectivos IDs");



        // DELETE: api/caracteristicas/{vehiculoId}/remove/{caracteristicaId}
        group.MapDelete("/{vehiculoId}/remove/{caracteristicaId}", async (
            Guid vehiculoId,
            Guid caracteristicaId,
            [FromServices] ICaracteristicaService caracteristicaService,
            CancellationToken cancellationToken) =>
        {
            var result = await caracteristicaService.RemoveVehiculoCaracteristicaAsync(vehiculoId, caracteristicaId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new { message = "Característica eliminada exitosamente del vehículo." })
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("RemoveVehiculoCaracteristica")
        .WithOpenApi()
        .WithSummary("Eliminar una característica de un vehículo")
        .WithDescription("Elimina la asociación de una característica con un vehículo por sus respectivos IDs");

        return app;
    }
}