using LanguageExt.Common;
using MediatR;

namespace Application.Features.Todo.Commands.DeleteRange
{
    public class DeleteTodosByStateAndUserIdCommand : IRequest<Result<string>>
    {
        public int IdUser { get; set; }
    }
}
