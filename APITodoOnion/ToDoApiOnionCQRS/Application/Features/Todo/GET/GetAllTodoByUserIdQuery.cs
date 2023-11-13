using Domain.Dtos.Todos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Todo.GET
{
    public class GetAllTodoByUserIdQuery : IRequest<Result<IEnumerable<TodoDto>>>
    {
        public int idUser { get; set; }
    }
}
