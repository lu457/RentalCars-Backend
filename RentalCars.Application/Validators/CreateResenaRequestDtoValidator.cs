using RentalCars.Application.DTOs.Resenas;
using FluentValidation;

namespace RentalCars.Application.Validators;

public class CreateResenaRequestDtoValidator : AbstractValidator<CreateResenaRequestDto>
{
    public CreateResenaRequestDtoValidator()
    {
        RuleFor(x => x.Comentario)
            .NotEmpty().WithMessage("El comentario es obligatorio")
            .MaximumLength(1000).WithMessage("El comentario no debe exceder los 1000 caracteres");

        RuleFor(x => x.Calificacion)
            .InclusiveBetween(1, 5).WithMessage("La calificación debe estar entre 1 y 5");

        RuleFor(x => x.VehiculoId)
            .NotEmpty().WithMessage("El ID del vehículo es obligatorio");

        RuleFor(x => x.ReservaId)
          .NotEmpty().WithMessage("El ID de la reserva es obligatorio");
    }
}
