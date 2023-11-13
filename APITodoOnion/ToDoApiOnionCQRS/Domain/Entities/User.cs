namespace Domain.Entities
{
    public class User
    {
        public int IdUser { get; set; }
        public string IdAccount { get; set; } = null!;
        //public ICollection<Todo>? UserTodo { get; set; }
    }
}
