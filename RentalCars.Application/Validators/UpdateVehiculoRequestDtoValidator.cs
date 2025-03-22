using RentalCars.Application.DTOs.Vehiculos;
using FluentValidation;

namespace RentalCars.Application.Validators
{
    public class UpdateVehiculoRequestDtoValidator : AbstractValidator<UpdateVehiculoRequestDto>
    {
        public UpdateVehiculoRequestDtoValidator()
        {
            Include(new CreateVehiculoRequestDtoValidator());

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del vehículo es obligatorio");
        }
    }
}
