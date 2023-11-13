using FluentValidation;

namespace Application.Features.Todo.Commands.Post
{
    public class PostTodoCommandHandlerValidator : AbstractValidator<PostTodoCommand>
    {
        public PostTodoCommandHandlerValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(r => r.IdUser)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido.");

            RuleFor(r => r.DateLimit)
                 .Must(BeAValidDate).When(r => r.DateLimit != null)
                 .WithMessage("El formato de fecha no es válido.");

            RuleFor(r => r.Description)
                .MaximumLength(255)
                .WithMessage("Máximo permitido es 255 carácteres");

   
        }
        private bool BeAValidDate(DateTime? date)
        {
            if (date.HasValue && date.Value >= DateTime.MinValue && date.Value <= DateTime.MaxValue)
            {
                return true;
            }
            return false;
        }


    }
}
