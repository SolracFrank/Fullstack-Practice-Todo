using Domain.Dtos.Todos;
using Domain.Entities;

namespace Domain.Dtos.Extensions
{
    public static class TodoExtension
    {
        public static TodoDto ToTodoDto(this Todo todo)
        {
            return new TodoDto
            {
                DateLimit = todo.DateLimit,
                Description = todo.Description,
                IdTodo = todo.IdTodo ,
                Estado = todo.Estado.ToString(),
                
            };
        }

        public static IEnumerable<TodoDto> ToTodoDto(this IEnumerable<Todo> todoList)
        {
            return todoList.Select(x => x.ToTodoDto());
        }
    }
}
