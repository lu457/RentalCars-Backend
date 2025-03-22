using FluentValidation;
using RentalCars.Application.DTOs.Sanciones;

namespace RentalCars.Application.Validators;

public class CreateSancionRequestDtoValidator : AbstractValidator<CreateSancionRequestDto>
{
    public CreateSancionRequestDtoValidator()
    {
        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(255).WithMessage("El motivo no puede superar los 255 caracteres.");

        RuleFor(x => x.Monto)
           .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

        RuleFor(x => x.ReservaId)
            .NotEmpty().WithMessage("El ID de la reserva es obligatorio.");
    }
}

