namespace Domain.Dtos.User.RefreshToken
{
    public class RefreshTokenRequest
    {
        public string OldRefreshToken { get; set; }
        public string UserId { get; set; }
    }
}
