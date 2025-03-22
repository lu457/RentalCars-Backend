using FluentValidation;
using RentalCars.Application.Interfaces;
using RentalCars.Application.Services;
using RentalCars.Infrastructure.Authentication.Models;
using RentalCars.Infrastructure.Authentication.Services;
using RentalCars.Infrastructure.Extensions;
using RentalCars.Infrastructure.Persistence;
using RentalCars.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalCars.Application.Validators;
using RentalCars.Domain.Entities;
using RentalCars.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Configuración de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IResenaRepository, ResenaRepository>();
builder.Services.AddScoped<IReglaRepository, ReglaRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<ISancionRepository, SancionRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<ICaracteristicaRepository, CaracteristicaRepository>();
builder.Services.AddScoped<IVehiculoFavoritoRepository, VehiculoFavoritoRepository>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

// Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<ICaracteristicaService, CaracteristicaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IReglaService, ReglaService>();
builder.Services.AddScoped<IResenaService, ResenaService>();
builder.Services.AddScoped<ISancionService, SancionService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Validadores
builder.Services.AddValidatorsFromAssemblyContaining<CreateReservaRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateVehiculoRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateResenaRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePagoRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateReservaRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateVehiculoRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSancionRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateSancionRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePagoRequestDtoValidator>();


// Configuración de autenticación y autorización
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Access-Control-Allow-Origin");
    });
});

// OpenAPI/Swagger
builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.UseCors("AllowAll");


// Configuración del pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalCars API V1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentalCars API V1");
    });
}
// Habilitar archivos estáticos
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de endpoints
app.MapAuthEndpoints();
app.MapVehiculoEndpoints();
app.MapReservaEndpoints();
app.MapResenaEndpoints();
app.MapSancionEndpoints();
app.MapPagoEndpoints();
app.MapReglaEndpoints();
app.MapCaracteristicaEndpoints();

app.Run();

