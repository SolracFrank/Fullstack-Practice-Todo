using LanguageExt.Common;
using MediatR;

namespace Application.Features.Todo.Commands.Post
{
    public class PostTodoCommand : IRequest<Result<string>>
    {
        public string? Description { get; set; }
        public DateTime? DateLimit { get; set; }
        public int IdUser { get; set; }
    }
}
