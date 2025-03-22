using FluentValidation;
using RentalCars.Application.DTOs.Sanciones;
using RentalCars.Domain.Enums;

namespace RentalCars.Application.Validators;

public class UpdateSancionRequestDtoValidator : AbstractValidator<UpdateSancionRequestDto>
{
    public UpdateSancionRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID de la sanción es obligatorio.");

        RuleFor(x => x.Estado)
            .NotNull().WithMessage("El estado es obligatorio.")
            .Must(EsEstadoValido).WithMessage("El estado de la sanción no es válido.");
    }

    private bool EsEstadoValido(string estado)
    {
        var estadosValidos = new[] { "Pendiente", "Pagado", "Cancelado"};
        return estadosValidos.Contains(estado);
    }
}
