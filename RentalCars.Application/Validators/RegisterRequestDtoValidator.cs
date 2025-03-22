using RentalCars.Application.DTOs.Auth;
using FluentValidation;

namespace RentalCars.Application.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio")
            .EmailAddress().WithMessage("Formato de correo electrónico inválido");

        RuleFor(x => x.Contraseña)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio")
            .MaximumLength(50).WithMessage("El apellido no debe exceder los 50 caracteres");

        RuleFor(x => x.Celular)
            .NotEmpty().WithMessage("El número de teléfono es obligatorio")
            .Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage("Formato de número de teléfono inválido");
    }
}

