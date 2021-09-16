using BC = BCrypt.Net.BCrypt;

namespace VinylApp.Domain.Services
{
    public static class EncryptionService
    {
        public static string Hash(string input)
        {
            var hashedPass = BC.HashPassword(input);
            return hashedPass;
        }

        public static bool Verify(string input, string hash)
        {
            var isVerified = BC.Verify(input, hash);
            return isVerified;
        }
    }
}
