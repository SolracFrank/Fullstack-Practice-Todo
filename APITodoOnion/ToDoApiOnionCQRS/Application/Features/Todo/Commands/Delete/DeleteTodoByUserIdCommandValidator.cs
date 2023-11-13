using Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Features.Todo.Commands.Delete
{
    public class DeleteTodoByUserIdCommandValidator : AbstractValidator<DeleteTodoByUserIdCommand>
    {
        public DeleteTodoByUserIdCommandValidator(IUnitOfWork _unitOfWork)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.IdUser)
              .NotEmpty()
              .WithMessage("El campo {PropertyName} es obligatorio")
              .GreaterThan(0)
              .WithMessage("El campo {PropertyName} debe ser un número entero positivo")
              .MustAsync(async (userId, cancellationToken) =>
                  await _unitOfWork.UserRepository.AnyAsync(user => user.IdUser == userId, cancellationToken))
              .WithMessage("El usuario no existe");

            RuleFor(r => r.IdTodo)
              .NotEmpty()
              .WithMessage("El campo {PropertyName} es obligatorio")
              .GreaterThan(0)
              .WithMessage("El campo {PropertyName} debe ser un número entero positivo")
              .MustAsync(async (IdTodo, cancellationToken) =>
                  await _unitOfWork.TodoRepository.AnyAsync(todo => todo.IdTodo == IdTodo, cancellationToken))
              .WithMessage("{PropertyName} no existe");

            RuleFor(r => r)
               .MustAsync(async (request, cancellationToken) =>
                   await _unitOfWork.TodoRepository.AnyAsync(todo => todo.IdTodo == request.IdTodo && todo.IdUser == request.IdUser, cancellationToken))
               .WithMessage("El Todo no pertenece al usuario especificado.");
        }
    }
}
