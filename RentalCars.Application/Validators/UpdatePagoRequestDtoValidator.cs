using FluentValidation;
using RentalCars.Application.DTOs.Pagos;

public class UpdatePagoRequestDtoValidator : AbstractValidator<UpdatePagoRequestDto>
{
    public UpdatePagoRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID del pago es obligatorio.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado del pago es obligatorio.")
            .Must(BeValidEstadoPago).WithMessage("Estado de pago no válido.");
    }

    private bool BeValidEstadoPago(string estado)
    {
        var estadosValidos = new[] { "Pendiente", "Aprobado", "Rechazado", "Cancelado" };
        return estadosValidos.Contains(estado);
    }
}
