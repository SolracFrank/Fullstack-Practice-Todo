using LanguageExt.Common;
using MediatR;

namespace Application.Features.Todo.Commands.Put
{
    public class PutTodoByUserIdCommand : IRequest<Result<string>>
    {
        public int IdTodo { get; set; }
        public int IdUser { get; set; }
        public string Estado { get; set; }
    }
}
