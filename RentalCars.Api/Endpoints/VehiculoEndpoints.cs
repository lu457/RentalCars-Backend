using System.Security.Claims;
using RentalCars.Application.DTOs.Vehiculos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RentalCars.Infrastructure.Extensions;
using RentalCars.Application.Interfaces;
using RentalCars.Infrastructure.Persistence;
using RentalCars.Domain.Entities;
using RentalCars.Domain.Enums;

namespace RentalCars.Api.Endpoints;

public static class VehiculoEndpoints
{
    public static IEndpointRouteBuilder MapVehiculoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vehiculos")
                      .WithTags("Vehiculos");

        // GET: api/vehiculos
        group.MapGet("/", async (
            [FromServices] IVehiculoService vehiculoService,
            CancellationToken cancellationToken) =>
        {
            var result = await vehiculoService.GetAllAsync(cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("GetVehiculos")
        .WithOpenApi()
        .WithSummary("Obtener todos los vehículos")
        .WithDescription("Recupera una lista de todos los vehículos disponibles");

        // GET: api/vehiculos/{id}
        group.MapGet("/{id}", async (
            Guid id,
            [FromServices] IVehiculoService vehiculoService,
            CancellationToken cancellationToken) =>
        {
            var result = await vehiculoService.GetByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .WithName("GetVehiculo")
        .WithOpenApi()
        .WithSummary("Obtener un vehículo por ID")
        .WithDescription("Recupera un vehículo específico por su ID");

        // POST: api/vehiculos
        group.MapPost("/", async (
            [FromBody] CreateVehiculoRequestDto request,
            [FromServices] IVehiculoService vehiculoService,
            IValidator<CreateVehiculoRequestDto> validator,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var propietarioId = user.GetUsuarioId();
            var result = await vehiculoService.CreateAsync(request, propietarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/vehiculos/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("CreateVehiculo")
        .WithOpenApi()
        .WithSummary("Registrar un nuevo vehículo")
        .WithDescription("Crea un nuevo vehículo en el sistema");

        // PUT: api/vehiculos/{id}
        group.MapPut("/{id}", async (
            Guid id,
            [FromBody] UpdateVehiculoRequestDto request,
            [FromServices] IVehiculoService vehiculoService,
            IValidator<UpdateVehiculoRequestDto> validator,
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

            var propietarioId = user.GetUsuarioId();
            var result = await vehiculoService.UpdateAsync(request, propietarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("UpdateVehiculo")
        .WithOpenApi()
        .WithSummary("Actualizar un vehículo")
        .WithDescription("Actualiza la información de un vehículo existente");

        // GET: api/vehiculos/filtrar
        group.MapGet("/filtrar", async (
            [FromQuery] string? ubicacion,
            [FromQuery] TipoDeVehiculo? tipoVehiculo,
            [FromServices] IVehiculoService vehiculoService,
            CancellationToken cancellationToken) =>
        {
            var result = await vehiculoService.FiltrarVehiculosAsync(ubicacion, tipoVehiculo, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("FiltrarVehiculos")
        .WithOpenApi()
        .WithSummary("Filtrar vehículos")
        .WithDescription("Filtra los vehículos según la ubicación y el tipo de vehículo.");



        // GET: api/vehiculos/propietario
        group.MapGet("/propietario", async (
            ClaimsPrincipal user,
            [FromServices] IVehiculoService vehiculoService,
            CancellationToken cancellationToken) =>
        {
            var propietarioId = user.GetUsuarioId();
            var result = await vehiculoService.GetVehiculosByPropietarioAsync(propietarioId, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("GetVehiculosByPropietario")
        .WithOpenApi()
        .WithSummary("Obtener vehículos por propietario")
        .WithDescription("Recupera todos los vehículos asociados al propietario autenticado");


        // DELETE: api/vehiculos/{id}
        group.MapDelete("/{id}", async (
            Guid id,
            [FromServices] IVehiculoService vehiculoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var propietarioId = user.GetUsuarioId();
            var result = await vehiculoService.DeleteAsync(id, propietarioId, cancellationToken);

            if (result.IsSuccess)
            {
                return Results.Ok(new { message = "Vehículo eliminado con éxito." });
            }

            return Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("DeleteVehiculo")
        .WithOpenApi()
        .WithSummary("Eliminar un vehículo")
        .WithDescription("Elimina un vehículo registrado en el sistema");

        // Agregar vehículo a favoritos
        group.MapPost("/{id}/favoritos", async (
            Guid id,
            [FromServices] IVehiculoService vehiculoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await vehiculoService.AgregarFavoritoAsync(usuarioId, id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new { message = "Vehículo agregado a favoritos" })
                : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("AgregarVehiculoFavorito")
        .WithOpenApi()
        .WithSummary("Agregar un vehículo a favoritos")
        .WithDescription("Permite al usuario agregar un vehículo a su lista de favoritos.");

        // Remover vehículo de favoritos
        group.MapDelete("/{id}/favoritos", async (
            Guid id,
            [FromServices] IVehiculoService vehiculoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await vehiculoService.RemoverFavoritoAsync(usuarioId, id, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(new { message = "Vehículo eliminado de favoritos" })
                : Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("RemoverVehiculoFavorito")
        .WithOpenApi()
        .WithSummary("Remover un vehículo de favoritos")
        .WithDescription("Permite al usuario eliminar un vehículo de su lista de favoritos.");

        // Verificar si un vehículo está en favoritos
        group.MapGet("/{id}/favoritos", async (
            Guid id,
            [FromServices] IVehiculoService vehiculoService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            var usuarioId = user.GetUsuarioId();
            var result = await vehiculoService.EsFavoritoAsync(usuarioId, id, cancellationToken);

            if (result.IsSuccess)
            {
                return Results.Ok(new { esFavorito = result.Value });
            }
            return Results.BadRequest(new { error = result.Error });
        })
        .RequireAuthorization()
        .WithName("EsVehiculoFavorito")
        .WithOpenApi()
        .WithSummary("Verificar si un vehículo está en favoritos")
        .WithDescription("Permite al usuario verificar si un vehículo está en su lista de favoritos.");


        group.MapPost("/{id}/images", async (
            Guid id,
            HttpContext context,
            [FromServices] IWebHostEnvironment env,
            [FromServices] ApplicationDbContext db,
            CancellationToken cancellationToken) =>
        {
            var files = context.Request.Form.Files;

            if (files.Count == 0)
            {
                return Results.BadRequest(new { error = "No se han subido imágenes" });
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var maxFileSize = 10 * 1024 * 1024;
            var uploadFolder = Path.Combine(env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var savedFiles = new List<string>();

            try
            {
                // Obtener esPrincipal y asociarlo a cada archivo
                var esPrincipalValues = context.Request.Form["esPrincipal"]
                    .Select(val => val == "1")
                    .ToList();

                var fileList = files
                    .Select((file, index) => new { File = file, EsPrincipal = esPrincipalValues.ElementAtOrDefault(index) })
                    .OrderBy(x => x.EsPrincipal ? 1 : 0)

                    .ToList();

                foreach (var item in fileList)
                {
                    var file = item.File;
                    var esPrincipal = item.EsPrincipal;

                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return Results.BadRequest(new { error = "Tipo de archivo no válido. Tipos permitidos: jpg, jpeg, png, webp" });
                    }

                    if (file.Length > maxFileSize)
                    {
                        return Results.BadRequest(new { error = "El tamaño del archivo excede el límite de 10MB" });
                    }

                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream, cancellationToken);

                    var fileUrl = $"/uploads/{fileName}";


                    var imagenVehiculo = new ImagenVehiculo
                    {
                        Id = Guid.NewGuid(),
                        Url = fileUrl,
                        VehiculoId = id,
                        EsPrincipal = esPrincipal
                    };

                    db.ImagenVehiculos.Add(imagenVehiculo);
                    savedFiles.Add(fileUrl);
                }

                await db.SaveChangesAsync(cancellationToken);

                return Results.Ok(new { message = "Imágenes subidas correctamente", files = savedFiles });
            }
            catch (Exception ex)
            {
                return Results.Problem("Ocurrió un error al subir las imágenes.", statusCode: 500);
            }
        })
        .WithName("UploadVehiculoImages")
        .WithOpenApi();


        return app;
    }
}

