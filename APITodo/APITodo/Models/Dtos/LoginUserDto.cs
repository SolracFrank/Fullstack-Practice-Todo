using System.ComponentModel.DataAnnotations;

namespace APITodo.Models.Dtos
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "UserName is mandatory")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
    }
}
