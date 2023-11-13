using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.Todo.GET
{
    public class GetAllTodoByUserIdQueryValidator :AbstractValidator<GetAllTodoByUserIdQuery>
    {
        public GetAllTodoByUserIdQueryValidator(IUnitOfWork _unitOfWork)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.idUser)
            .NotEmpty()
            .WithMessage("El campo {PropertyName} es obligatorio")
            .GreaterThan(0)
            .WithMessage("El campo {PropertyName} debe ser un número entero positivo")
            .MustAsync(async (userId, cancellationToken) =>
                await _unitOfWork.UserRepository.AnyAsync(user => user.IdUser == userId, cancellationToken))
            .WithMessage("El usuario no existe");

        }
    }
}
