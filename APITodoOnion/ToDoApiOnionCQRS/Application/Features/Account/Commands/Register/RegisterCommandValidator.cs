using FluentValidation;

namespace Application.Features.Account.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;


            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder {MaxLength}");

            RuleFor(p => p.LastName)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
               .MaximumLength(50).WithMessage("{PropertyName} no debe exceder {MaxLength}");

            RuleFor(p => p.Email)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
               .EmailAddress().WithMessage("{PropertyName} debe ser una direccion de correo válida")
               .MaximumLength(100).WithMessage("{PropertyName} no debe exceder {MaxLength}");

            RuleFor(p => p.UserName)
              .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
              .MaximumLength(10).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");

            RuleFor(p => p.Password)
              .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
              .MaximumLength(15).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres");

            RuleFor(p => p.ConfirmPassword)
                 .NotEmpty().WithMessage("{PropertyName} no puede ser vacío")
                 .MaximumLength(15).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres")
                 .Equal(p => p.Password).WithMessage("{PropertyName} debe ser igual a Password");
        }
    }
}
