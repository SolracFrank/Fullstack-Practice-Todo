using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITodo.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
       // #pragma warning disable/restore CS8618
        public string Description { get; set; }
        public bool State { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
