using System.ComponentModel.DataAnnotations;

namespace APITodo.Models.Dtos
{
    public class TodoDto
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public bool State { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string userId { get; set; }
    }
}
