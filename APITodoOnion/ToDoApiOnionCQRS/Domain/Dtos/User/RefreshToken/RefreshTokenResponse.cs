namespace Domain.Dtos.User.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
