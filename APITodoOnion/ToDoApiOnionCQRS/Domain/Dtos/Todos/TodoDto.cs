namespace Domain.Dtos.Todos
{
    public class TodoDto
    {
        public int IdTodo { get; set; }
        public string? Description { get; set; }
        public DateTime? DateLimit { get; set; }
        public string Estado { get; set; }

    }
}
