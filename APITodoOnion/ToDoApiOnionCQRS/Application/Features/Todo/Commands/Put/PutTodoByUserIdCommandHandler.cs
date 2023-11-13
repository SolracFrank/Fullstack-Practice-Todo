using Domain.Interfaces;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Todo.Commands.Put
{
    public class PutTodoByUserIdCommandHandler : IRequestHandler<PutTodoByUserIdCommand, Result<string>>
    {
        private ILogger<PutTodoByUserIdCommandHandler> _logger;
        private IUnitOfWork _unitOfWork;

        public PutTodoByUserIdCommandHandler(ILogger<PutTodoByUserIdCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(PutTodoByUserIdCommand request, CancellationToken cancellationToken)
        {
        
            var validator = new PutTodoByUserIdCommandValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Validation errors with Todo");


                var validationException = new ValidationException(validationResult.Errors);
                return new Result<string>(new ValidationException(validationException.Errors));
            }

   
            var todo = await _unitOfWork.TodoRepository.FirstOrDefaultAsync(x => x.IdTodo == request.IdTodo && x.IdUser == request.IdUser,cancellationToken);

            todo.Estado = request.Estado;

            _unitOfWork.TodoRepository.Update(todo);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if(!result)
            {
                _logger.LogInformation("An Database error has ocurred trying to update TODO");
                return new Result<string>("Ha habido un error al actualizar el TODO en la base de datos");
            }

            return new Result<string>("Actualizado TODO satisfactoriamente");
        }
    }
}
