using FluentValidation;
using RentalCars.Application.DTOs.Reservas;

namespace RentalCars.Application.Validators
{
    public class UpdateReservaRequestDtoValidator : AbstractValidator<UpdateReservaRequestDto>
    {
        public UpdateReservaRequestDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID de la reserva es obligatorio.");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .Must(SerEstadoValido).WithMessage("Estado de reserva no válido.");
        }

        private bool SerEstadoValido(string estado)
        {
            var estadosValidos = new[] { "Pendiente", "Confirmada", "Completada", "Cancelada" };
            return estadosValidos.Contains(estado);
        }
    }
}

