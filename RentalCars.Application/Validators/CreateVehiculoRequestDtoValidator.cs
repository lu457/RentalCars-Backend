using RentalCars.Application.DTOs.Vehiculos;
using FluentValidation;

namespace RentalCars.Application.Validators
{
    public class CreateVehiculoRequestDtoValidator : AbstractValidator<CreateVehiculoRequestDto>
    {
        public CreateVehiculoRequestDtoValidator()
        {
            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("La marca del vehículo es obligatoria")
                .MaximumLength(50).WithMessage("La marca no debe exceder los 50 caracteres");

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo del vehículo es obligatorio")
                .MaximumLength(50).WithMessage("El modelo no debe exceder los 50 caracteres");

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("El año del vehículo debe ser mayor a 1900")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage($"El año del vehículo no puede ser mayor a {DateTime.Now.Year}");

            RuleFor(x => x.PrecioPorDia)
                .GreaterThan(0).WithMessage("El precio por día debe ser mayor a 0");

            RuleFor(x => x.Tipo)
                .NotEmpty().WithMessage("El tipo de vehículo es obligatorio")
                .MaximumLength(50).WithMessage("El tipo de vehículo no debe exceder los 50 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(2000).WithMessage("La descripción no debe exceder los 2000 caracteres");

            RuleFor(x => x.Motor)
                .NotEmpty().WithMessage("El motor es obligatoria")
                .MaximumLength(2000).WithMessage("El motor no debe exceder los 100 caracteres");

            RuleFor(x => x.Cilindros)
                .GreaterThanOrEqualTo(1).WithMessage("El número de cilindros debe ser al menos 1")
                .LessThanOrEqualTo(16).WithMessage("El número de cilindros no debe exceder los 16");

            RuleFor(x => x.Puertas)
                .GreaterThanOrEqualTo(2).WithMessage("El número de puertas debe ser al menos 2")
                .LessThanOrEqualTo(6).WithMessage("El número de puertas no debe exceder los 6");

            RuleFor(x => x.CapacidadPasajeros)
                .GreaterThan(0).WithMessage("La capacidad de pasajeros debe ser mayor a 0")
                .LessThanOrEqualTo(50).WithMessage("La capacidad de pasajeros no debe exceder los 50");

            RuleFor(x => x.Combustible)
                .NotEmpty().WithMessage("El tipo de combustible es obligatorio")
                .MaximumLength(50).WithMessage("El tipo de combustible no debe exceder los 50 caracteres");

            RuleFor(x => x.Transmision)
                .NotEmpty().WithMessage("El tipo de transmisión es obligatorio")
                .MaximumLength(50).WithMessage("El tipo de transmisión no debe exceder los 50 caracteres");

        }
    }
}
