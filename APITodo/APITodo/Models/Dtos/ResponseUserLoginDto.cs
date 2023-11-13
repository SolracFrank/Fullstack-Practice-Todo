namespace APITodo.Models.Dtos
{
    public class ResponseUserLoginDto
    {
        public DataUserDto User { get; set; }
        public string Token { get; set; }
    }
}
