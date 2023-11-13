using LanguageExt.Common;
using MediatR;

namespace Application.Features.Todo.Commands.Delete
{
    public class DeleteTodoByUserIdCommand : IRequest<Result<string>>
    {
        public int IdTodo { get; set; }
        public int IdUser { get; set; }
    }
}
