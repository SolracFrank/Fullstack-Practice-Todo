using Application.Features.Todo.Commands.Post;
using Domain.Interfaces;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Todo.Commands.Delete
{
    public class DeleteTodoByUserIdCommandHandler : IRequestHandler<DeleteTodoByUserIdCommand, Result<string>>
    {
        private readonly ILogger<DeleteTodoByUserIdCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTodoByUserIdCommandHandler(ILogger<DeleteTodoByUserIdCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteTodoByUserIdCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteTodoByUserIdCommandValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Validation errors with Todo");
      

                var validationException = new ValidationException(validationResult.Errors);

                return new Result<string>(new ValidationException(validationException.Errors));
            }
            _logger.LogInformation("Deleting Todo");

            var todo = await _unitOfWork.TodoRepository.FirstOrDefaultAsync(t =>
            t.IdTodo == request.IdTodo && t.IdUser == request.IdUser
            , cancellationToken, true);

             _unitOfWork.TodoRepository.Remove(todo);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if(!result)
            {

                _logger.LogInformation("Error deleting Todo");

                var validationException = new ValidationException(validationResult.Errors);
              
                return new Result<string>(new ValidationException(validationException.Errors));
            }

            return new Result<string>("Todo eliminado satisfactoriamente");
        }
    }
}
        