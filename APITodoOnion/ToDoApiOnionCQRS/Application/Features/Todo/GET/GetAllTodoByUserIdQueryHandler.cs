using Domain.Dtos.Extensions;
using Domain.Dtos.Todos;
using Domain.Interfaces;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Todo.GET
{
    public class GetAllTodoByUserIdQueryHandler : IRequestHandler<GetAllTodoByUserIdQuery, Result<IEnumerable<TodoDto>>>
    {
        private readonly ILogger<GetAllTodoByUserIdQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTodoByUserIdQueryHandler(ILogger<GetAllTodoByUserIdQueryHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<TodoDto>>> Handle(GetAllTodoByUserIdQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetAllTodoByUserIdQueryValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);


            if (!validationResult.IsValid)
            {
                _logger.LogInformation($"User with {request.idUser} doesn't exist");

                var validationException = new ValidationException(validationResult.Errors);
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                var allErrors = string.Join(", ", errors);

                return new  Result <IEnumerable<TodoDto >> (new ValidationException(allErrors));
            }

            var todo = await _unitOfWork.TodoRepository.WhereAsync(t => t.IdUser == request.idUser, cancellationToken, true);


            return new Result<IEnumerable<TodoDto>>(todo.ToTodoDto().OrderBy(x => x.IdTodo));
        }
    }
}
