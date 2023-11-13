using Domain.Interfaces;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Todo.Commands.Post
{
    public class PostTodoCommandHandler : IRequestHandler<PostTodoCommand, Result<string>>
    {
        private readonly ILogger<PostTodoCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public PostTodoCommandHandler(ILogger<PostTodoCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(PostTodoCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostTodoCommandHandlerValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);


            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Validation errors with Todo");

                var validationException = new ValidationException(validationResult.Errors);
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                var allErrors = string.Join(", ", errors);

                return new Result<string>(new ValidationException(allErrors));
            }

            var todo = new Domain.Entities.Todo
            {
                IdUser = request.IdUser,
                DateLimit = request?.DateLimit,
                Description = request?.Description,
            };

            _logger.LogInformation("Adding new Todo");

            await _unitOfWork.TodoRepository.AddAsync(todo, cancellationToken);

            var insertResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (!insertResult)
            {
                _logger.LogInformation("An error has ocurred at inserting a new TODO");
                return new Result<string>("Error al agregar un nuevo Todo");
            }

            return new Result<string>("Todo exitoso");
        }
    }
}
