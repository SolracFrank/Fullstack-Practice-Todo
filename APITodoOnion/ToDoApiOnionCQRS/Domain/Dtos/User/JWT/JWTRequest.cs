namespace Domain.Dtos.User.JWT
{
    public class JWTRequest
    {
        public string userId { get; set; }
        public string RefreshToken { get; set; }
        public string OldJwtToken { get; set; }
    }
}
