using System.ComponentModel.DataAnnotations;

namespace APITodo.Models.Dtos
{
    public class CreateTodoDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public bool State { get; set; }

    }
}
