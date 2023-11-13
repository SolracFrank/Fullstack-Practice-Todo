using System.Security.Cryptography;

namespace Infrastructure.Helpers
{
    public class RefreshTokenHelper
    {
        public static string RandomTokenString()
        {
            using var rngCryptoServicesProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServicesProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
