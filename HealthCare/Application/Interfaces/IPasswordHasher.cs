using BCrypt.Net;

namespace HealthCare.Application.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Hash a plain text password using BCrypt
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verify a plain text password against a hashed password
    /// </summary>
    /// <param name="password">Plain text password to verify</param>
    /// <param name="hash">Hashed password from database</param>
    /// <returns>True if password matches, false otherwise</returns>
    bool VerifyPassword(string password, string hash);
}
