namespace HealthCare.Application.Interfaces;

/// <summary>
/// Password validation result
/// </summary>
public class PasswordValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

/// <summary>
/// Password validation interface for enforcing password security requirements
/// </summary>
public interface IPasswordValidator
{
    /// <summary>
    /// Validate password against security requirements
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>Validation result with status and message</returns>
    PasswordValidationResult ValidatePassword(string password);

    /// <summary>
    /// Get password requirements for UI display
    /// </summary>
    /// <returns>List of password requirements</returns>
    List<string> GetPasswordRequirements();
}

