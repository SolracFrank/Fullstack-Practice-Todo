using System.ComponentModel.DataAnnotations;

namespace APITodo.Models.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "User is mandatory")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
