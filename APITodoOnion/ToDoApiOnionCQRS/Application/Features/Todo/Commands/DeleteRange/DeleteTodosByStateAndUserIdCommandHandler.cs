using Domain.Exceptions;
using Domain.Interfaces;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ValidationException = FluentValidation.ValidationException;

namespace Application.Features.Todo.Commands.DeleteRange
{
    public class DeleteTodosByStateAndUserIdCommandHandler : IRequestHandler<DeleteTodosByStateAndUserIdCommand, Result<string>>
    {
        readonly private ILogger<DeleteTodosByStateAndUserIdCommand> _logger;
        readonly private IUnitOfWork _unitOfWork;

        public DeleteTodosByStateAndUserIdCommandHandler(ILogger<DeleteTodosByStateAndUserIdCommand> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteTodosByStateAndUserIdCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteTodosByStateAndUserIdCommandValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Validation errors with Todo");

                return new Result<string>(new ValidationException(validationResult.Errors));
            }

            var todoList = await _unitOfWork.TodoRepository.WhereAsync(t => t.IdUser == request.IdUser && t.Estado == "Completado", cancellationToken);

            var peoe = "like";
            if(todoList.IsNullOrEmpty())
            {
                return new Result<string>("No hay tareas completadas para eliminar");
            }

             _unitOfWork.TodoRepository.RemoveRange(todoList);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            if(!result)
            {
                _logger.LogInformation("Ocurrió un error al eliminar los Todo");

                return new Result<string>(new ApiExceptions("Error desconocido al eliminar TODO completados"));
            }

            return new Result<string>("TODO completados eliminados exitosamente");
        }
    }
}
