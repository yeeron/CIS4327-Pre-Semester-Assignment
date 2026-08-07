using Microsoft.AspNetCore.Identity;

namespace VolunteerMS.Utilities
{
    public static class PasswordHasher
    {
        // We initialize the native hasher once globally inside the static class
        private static readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

        // Reusable method to encrypt passwords
        public static string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        // Reusable method to check passwords during login
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
