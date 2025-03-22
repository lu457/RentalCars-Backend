using Microsoft.OpenApi.Models;
using RentalCars.Application.DTOs.Auth;
using RentalCars.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace RentalCars.Api.Endpoints;

public static class AuthEndpoints
{
    // Método para mapear los endpoints de autenticación
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
                      .WithTags("Autenticación");

        // Endpoint para el inicio de sesión

        group.MapPost("/login", async (
            [FromBody] LoginRequestDto request,
            [FromServices] IAuthService authService,
            IValidator<LoginRequestDto> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var result = await authService.LoginAsync(request, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("Login")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Autenticar usuario";
            operation.Description = "Autentica un usuario y devuelve un token JWT para solicitudes posteriores.";

            operation.RequestBody.Content["application/json"].Example =
                new OpenApiString("{\"email\": \"juanperez@example.com\", \"contraseña\": \"superchichico\"}");

            operation.Responses[StatusCodes.Status200OK.ToString()].Content["application/json"].Example = new OpenApiObject
            {
                ["Token"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"),
                ["ExpiresIn"] = new OpenApiInteger(3600)
            };

            return operation;
        });
        // Endpoint para el registro de nuevos usuarios

        group.MapPost("/register", async (
            [FromBody] RegisterRequestDto request,
            [FromServices] IAuthService authService,
            IValidator<RegisterRequestDto> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var result = await authService.RegisterAsync(request, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/usuarios/{result.Value.Email}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        })
        .WithName("Registro")
        .WithOpenApi()
        .WithSummary("Registrar nuevo usuario")
        .WithDescription("Registra un nuevo usuario en el sistema");

        return app;
    }
}
