using FluentValidation;
using RentalCars.Application.DTOs.Pagos;

public class CreatePagoRequestDtoValidator : AbstractValidator<CreatePagoRequestDto>
{
    public CreatePagoRequestDtoValidator()
    {
        RuleFor(x => x.ReservaId)
            .NotEmpty().WithMessage("El ID de la reserva es obligatorio.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

        RuleFor(x => x.MetodoPago)
            .NotEmpty().WithMessage("El método de pago es obligatorio.")
            .Must(BeValidMetodoPago).WithMessage("Método de pago no válido.");
    }

    private bool BeValidMetodoPago(string metodoPago)
    {
        var metodosValidos = new[] { "TarjetaCredito", "Paypal", "TarjetaDebito", "Yape" };
        return metodosValidos.Contains(metodoPago);
    }
}
