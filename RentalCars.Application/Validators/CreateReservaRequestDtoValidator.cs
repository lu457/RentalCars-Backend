using FluentValidation;
using RentalCars.Application.DTOs.Reservas;
using System;

namespace RentalCars.Application.Validators
{
    public class CreateReservaRequestDtoValidator : AbstractValidator<CreateReservaRequestDto>
    {
        public CreateReservaRequestDtoValidator()
        {
            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.")
                .Must(SerFechaFutura).WithMessage("La fecha de inicio debe ser en el futuro.");

            RuleFor(x => x.FechaFin)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria.")
                .Must(SerFechaFutura).WithMessage("La fecha de fin debe ser en el futuro.")
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            RuleFor(x => x.VehiculoId)
                .NotEmpty().WithMessage("El ID del vehículo es obligatorio.");
        }

        private bool SerFechaFutura(DateTime fecha)
        {
            return fecha > DateTime.Now;
        }
    }
}
