using RentalCars.Application.DTOs.Reglas;
using RentalCars.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RentalCars.Api.Endpoints;

public static class ReglaEndpoints
{
    public static IEndpointRouteBuilder MapReglaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reglas")
                      .WithTags("Reglas");

        // GET: api/reglas
        group.MapGet("/", async (
            [FromServices] IReglaService reglaService,
            CancellationToken cancellationToken) =>
        {
            var result = await reglaService.GetAllReglasAsync(cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetAllReglas")
        .WithOpenApi()
        .WithSummary("Obtener todas las reglas")
        .WithDescription("Recupera todas las reglas disponibles en el sistema");

        // GET: api/reglas/{id}
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] IReglaService reglaService,
            CancellationToken cancellationToken) =>
        {
            var result = await reglaService.GetReglaByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetReglaById")
        .WithOpenApi()
        .WithSummary("Obtener una regla por ID")
        .WithDescription("Recupera una regla específica por su ID");

        // POST: api/reglas
        group.MapPost("/", async (
            CreateReglaDto createReglaDto,
            [FromServices] IReglaService reglaService,
            CancellationToken cancellationToken) =>
        {
            var result = await reglaService.CreateReglaAsync(createReglaDto, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/reglas/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("CreateRegla")
        .WithOpenApi()
        .WithSummary("Crear una nueva regla")
        .WithDescription("Crea una nueva regla con los detalles especificados");

        return app;
    }
}
