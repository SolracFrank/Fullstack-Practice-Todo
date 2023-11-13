namespace Domain.Entities
{
    public class Todo
    {
        public int IdTodo { get; set; }
        public string? Description { get; set; }
        public DateTime? DateLimit { get; set; }
        public enum TipoEstado { Activo, Completado}
        public string Estado { get; set; } = TipoEstado.Activo.ToString();
        public bool IsExpired => DateTime.UtcNow >= DateLimit;
        public int IdUser { get; set; }
    }
}
