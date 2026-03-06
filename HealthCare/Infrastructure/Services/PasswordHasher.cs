using BCrypt.Net;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly ILogger<PasswordHasher> _logger;

    public PasswordHasher(ILogger<PasswordHasher> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Hash a plain text password using BCrypt with salt
    /// BCrypt automatically generates a salt and includes it in the hash
    /// </summary>
    /// <param name="password">Plain text password to hash</param>
    /// <returns>BCrypt hashed password (includes salt)</returns>
    public string HashPassword(string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            // BCrypt.Net uses work factor 12 by default (2^12 iterations)
            // This is the recommended security level as of 2024
            // Higher work factors take longer but are more secure against brute force
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            
            _logger.LogInformation("Password hashed successfully");
            return hashedPassword;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error hashing password");
            throw;
        }
    }

    /// <summary>
    /// Verify a plain text password against a BCrypt hash
    /// Uses BCrypt's built-in comparison that's resistant to timing attacks
    /// </summary>
    /// <param name="password">Plain text password to verify</param>
    /// <param name="hash">BCrypt hash from database</param>
    /// <returns>True if password matches the hash, false otherwise</returns>
    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Password verification attempted with null/empty password");
                return false;
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                _logger.LogWarning("Password verification attempted with null/empty hash");
                return false;
            }

            // BCrypt.Verify is timing-attack resistant
            // It uses constant-time comparison to prevent timing analysis attacks
            bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
            
            if (isValid)
            {
                _logger.LogInformation("Password verified successfully");
            }
            else
            {
                _logger.LogWarning("Password verification failed - incorrect password");
            }

            return isValid;
        }
        catch (SaltParseException ex)
        {
            _logger.LogError(ex, "Invalid hash format during password verification");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying password");
            return false;
        }
    }
}
