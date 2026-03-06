using System.Text.RegularExpressions;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

/// <summary>
/// Password validator using industry-standard security requirements
/// </summary>
public class PasswordValidator : IPasswordValidator
{
    private readonly ILogger<PasswordValidator> _logger;

    // Password validation rules (configurable)
    public const int MinimumLength = 8;
    public const int MaximumLength = 128;
    public const bool RequireUppercase = true;
    public const bool RequireLowercase = true;
    public const bool RequireDigits = true;
    public const bool RequireSpecialCharacters = true;

    // Special characters allowed
    private const string AllowedSpecialCharacters = "!@#$%^&*()_+-=[]{}|;':\"<>,.?/";

    public PasswordValidator(ILogger<PasswordValidator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Validate password against all security requirements
    /// </summary>
    public PasswordValidationResult ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Password validation failed: null or empty password");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password is required and cannot be empty" 
            };
        }

        // Check length
        if (password.Length < MinimumLength)
        {
            _logger.LogWarning($"Password validation failed: too short (min {MinimumLength})");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = $"Password must be at least {MinimumLength} characters long" 
            };
        }

        if (password.Length > MaximumLength)
        {
            _logger.LogWarning($"Password validation failed: too long (max {MaximumLength})");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = $"Password must not exceed {MaximumLength} characters" 
            };
        }

        // Check for uppercase letters
        if (RequireUppercase && !password.Any(char.IsUpper))
        {
            _logger.LogWarning("Password validation failed: no uppercase letters");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password must contain at least one uppercase letter (A-Z)" 
            };
        }

        // Check for lowercase letters
        if (RequireLowercase && !password.Any(char.IsLower))
        {
            _logger.LogWarning("Password validation failed: no lowercase letters");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password must contain at least one lowercase letter (a-z)" 
            };
        }

        // Check for digits
        if (RequireDigits && !password.Any(char.IsDigit))
        {
            _logger.LogWarning("Password validation failed: no digits");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password must contain at least one digit (0-9)" 
            };
        }

        // Check for special characters
        if (RequireSpecialCharacters && !password.Any(c => AllowedSpecialCharacters.Contains(c)))
        {
            _logger.LogWarning("Password validation failed: no special characters");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = $"Password must contain at least one special character: {AllowedSpecialCharacters}" 
            };
        }

        // Check for common patterns (prevent weak passwords)
        if (IsCommonPattern(password))
        {
            _logger.LogWarning("Password validation failed: matches common pattern");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password is too common. Please use a more unique password" 
            };
        }

        // Check for sequential characters (e.g., 123, abc, qwerty)
        if (ContainsSequentialCharacters(password))
        {
            _logger.LogWarning("Password validation failed: contains sequential characters");
            return new PasswordValidationResult 
            { 
                IsValid = false, 
                ErrorMessage = "Password contains sequential characters (e.g., 123, abc). Please avoid sequences" 
            };
        }

        _logger.LogInformation("Password validation successful");
        return new PasswordValidationResult 
        { 
            IsValid = true, 
            ErrorMessage = "Password is valid" 
        };
    }

    /// <summary>
    /// Get password requirements for UI display
    /// </summary>
    public List<string> GetPasswordRequirements()
    {
        var requirements = new List<string>
        {
            $"At least {MinimumLength} characters long",
            $"No more than {MaximumLength} characters"
        };

        if (RequireUppercase)
            requirements.Add("At least one uppercase letter (A-Z)");

        if (RequireLowercase)
            requirements.Add("At least one lowercase letter (a-z)");

        if (RequireDigits)
            requirements.Add("At least one digit (0-9)");

        if (RequireSpecialCharacters)
            requirements.Add($"At least one special character: {AllowedSpecialCharacters}");

        requirements.Add("No common patterns (password, 123456, qwerty, etc.)");
        requirements.Add("No sequential characters (abc, 123, etc.)");

        return requirements;
    }

    /// <summary>
    /// Check if password matches common weak patterns
    /// </summary>
    private bool IsCommonPattern(string password)
    {
        var commonPatterns = new[]
        {
            "password", "123456", "qwerty", "abc123", "admin", "letmein",
            "welcome", "monkey", "dragon", "master", "sunshine", "princess"
        };

        var lowerPassword = password.ToLower();
        return commonPatterns.Any(pattern => lowerPassword.Contains(pattern));
    }

    /// <summary>
    /// Check if password contains sequential characters
    /// </summary>
    private bool ContainsSequentialCharacters(string password)
    {
        // Check for 3+ consecutive numbers or letters
        var patterns = new[]
        {
            @"(?:012|123|234|345|456|567|678|789|890)",  // Numbers
            @"(?:abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz)",  // Letters
            @"(?:ABC|BCD|CDE|DEF|EFG|FGH|GHI|HIJ|IJK|JKL|KLM|LMN|MNO|NOP|OPQ|PQR|QRS|RST|STU|TUV|UVW|VWX|WXY|XYZ)"  // Uppercase
        };

        foreach (var pattern in patterns)
        {
            if (Regex.IsMatch(password, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }
}
