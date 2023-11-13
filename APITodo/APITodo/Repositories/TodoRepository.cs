using APITodo.Data;
using APITodo.Models;
using APITodo.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace APITodo.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDBContext _bd;
        public TodoRepository(ApplicationDBContext bd)
        {
                _bd = bd;
        }
        public bool CreateTodo(Todo todo, string userId)
        {
            todo.CreatedDate = DateTime.Now;
            todo.UpdatedTime = DateTime.Now;
            todo.UserId = userId;
            _bd.todos.Add(todo);
            return Saved();
        }

        public bool DeleteCompletedTodo(bool state, string userId)
        {
            var completedTodos = _bd.todos.Where(todo => todo.State == state && todo.UserId == userId).ToList();

            _bd.todos.RemoveRange(completedTodos);

            return Saved();
        }


        public bool DeleteTodo(Todo todo)
        {
            _bd.todos.Remove(todo);
            return Saved();
        }

        public Todo GetTodo(string userId, int todoId)
        {
            var getTodo = _bd.todos
                .Include(todo => todo.User)
                .FirstOrDefault(todo => todo.Id == todoId && todo.UserId == userId);


            return getTodo;
        }

        public ICollection<Todo> GetTodos(string userId)
        {
            return _bd.todos
                .Include(todo => todo.User)
                .Where(todo => todo.UserId == userId).ToList();
        }
        //For Post; Delete and Patch
        public bool Saved()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTodo(Todo todo)
        {
            todo.UpdatedTime = DateTime.Now;
            _bd.Update(todo);

            return Saved();
        }
    }
}
