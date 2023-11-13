using APITodo.Models;

namespace APITodo.Repositories.IRepositories
{
    public interface ITodoRepository
    {
        ICollection<Todo> GetTodos(string userId);
        Todo GetTodo(string userId, int todoId);
        bool CreateTodo(Todo todo, string userId);
        bool UpdateTodo(Todo todo);
        bool DeleteTodo(Todo todo);
        bool DeleteCompletedTodo(bool state, string userId);
        bool Saved();
      
    }
}
