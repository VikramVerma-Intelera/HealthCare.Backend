using Microsoft.AspNetCore.Mvc;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;
using HealthCare.Application.Common;
using Microsoft.Extensions.Configuration;

namespace HealthCare.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    // In-memory user store with HASHED passwords for demonstration
    // In production, replace with actual user database with hashed passwords stored
    private static readonly Dictionary<string, string> ValidUsers = new()
    {
        // Each password here should be a BCrypt hash in production
        // For demo purposes, we'll hash them on first run
        // In real app: passwords are hashed when user registers and stored as hash in DB
        { "admin", "" },        // Will be hashed on first use
        { "doctor", "" },       // Will be hashed on first use
        { "patient", "" },      // Will be hashed on first use
        { "user", "" }          // Will be hashed on first use
    };

    // Plain passwords for hashing on startup (FOR DEVELOPMENT ONLY!)
    // In production, users register with passwords, which are hashed and stored
    private static readonly Dictionary<string, string> PlainPasswords = new()
    {
        { "admin", "admin123" },
        { "doctor", "doctor123" },
        { "patient", "patient123" },
        { "user", "password123" }
    };

    private static bool _passwordsInitialized = false;
    private static readonly object _lockObject = new object();

    public AuthController(
        ITokenService tokenService, 
        IPasswordHasher passwordHasher,
        IPasswordValidator passwordValidator,
        IConfiguration configuration, 
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _passwordValidator = passwordValidator;
        _configuration = configuration;
        _logger = logger;

        // Initialize hashed passwords once (thread-safe)
        if (!_passwordsInitialized)
        {
            lock (_lockObject)
            {
                if (!_passwordsInitialized)
                {
                    InitializeHashedPasswords();
                    _passwordsInitialized = true;
                }
            }
        }
    }

    /// <summary>
    /// Initialize hashed passwords for demo users
    /// In production, passwords are hashed during user registration
    /// </summary>
    private void InitializeHashedPasswords()
    {
        try
        {
            foreach (var user in PlainPasswords)
            {
                // Hash the plain password using BCrypt
                ValidUsers[user.Key] = _passwordHasher.HashPassword(user.Value);
            }
            _logger.LogInformation("Demo user passwords initialized with BCrypt hashing");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing hashed passwords");
        }
    }

    /// <summary>
    /// Login endpoint - generates JWT token
    /// </summary>
    /// <param name="loginRequest">Username and password</param>
    /// <returns>JWT token if credentials are valid</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                _logger.LogWarning("Login attempt with missing credentials");
                return Unauthorized(ApiResponse<object>.ErrorResponse("Username and password are required", 401));
            }

            // Check if user exists
            if (!ValidUsers.TryGetValue(loginRequest.Username, out var hashedPassword))
            {
                _logger.LogWarning($"Failed login attempt - user not found: {loginRequest.Username}");
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid username or password", 401));
            }

            // Verify password using BCrypt (timing-attack resistant)
            // BCrypt.Verify compares the plain text password with the stored hash
            if (!_passwordHasher.VerifyPassword(loginRequest.Password, hashedPassword))
            {
                _logger.LogWarning($"Failed login attempt - invalid password for user: {loginRequest.Username}");
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid username or password", 401));
            }

            // Generate JWT token
            var token = _tokenService.GenerateToken(loginRequest.Username);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var tokenResponse = new TokenResponse
            {
                Token = token,
                Username = loginRequest.Username,
                ExpiresAt = expiresAt,
                TokenType = "Bearer"
            };

            _logger.LogInformation($"User {loginRequest.Username} logged in successfully");
            return Ok(ApiResponse<TokenResponse>.SuccessResponse(tokenResponse, "Login successful", 200));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during login", 500));
        }
    }

    /// <summary>
    /// Refresh token endpoint - generates a new JWT token
    /// </summary>
    /// <returns>New JWT token</returns>
    [HttpPost("refresh")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public IActionResult RefreshToken()
    {
        try
        {
            var username = User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Token refresh attempt without authentication");
                return Unauthorized(ApiResponse<object>.ErrorResponse("User not authenticated", 401));
            }

            // Generate new token
            var token = _tokenService.GenerateToken(username);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var tokenResponse = new TokenResponse
            {
                Token = token,
                Username = username,
                ExpiresAt = expiresAt,
                TokenType = "Bearer"
            };

            _logger.LogInformation($"Token refreshed for user: {username}");
            return Ok(ApiResponse<TokenResponse>.SuccessResponse(tokenResponse, "Token refreshed successfully", 200));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during token refresh", 500));
        }
    }

    /// <summary>
    /// Get test credentials for demonstration
    /// </summary>
    /// <returns>List of available test users</returns>
    [HttpGet("test-credentials")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult GetTestCredentials()
    {
        var testCredentials = new
        {
            note = "For demonstration only. Replace with actual user database in production.",
            users = ValidUsers.Select(x => new { username = x.Key }).ToList()
        };

        return Ok(ApiResponse<object>.SuccessResponse(testCredentials, "Test credentials retrieved", 200));
    }

    /// <summary>
    /// Get password requirements for UI validation
    /// </summary>
    /// <returns>List of password requirements</returns>
    [HttpGet("password-requirements")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult GetPasswordRequirements()
    {
        var requirements = _passwordValidator.GetPasswordRequirements();
        var response = new
        {
            requirements = requirements,
            minimumLength = 8,
            maximumLength = 128,
            note = "These requirements ensure strong password security"
        };

        return Ok(ApiResponse<object>.SuccessResponse(response, "Password requirements retrieved", 200));
    }

    /// <summary>
    /// Validate password against security requirements (for client-side validation)
    /// </summary>
    /// <param name="validatePasswordRequest">Password to validate</param>
    /// <returns>Validation result</returns>
    [HttpPost("validate-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult ValidatePassword([FromBody] ValidatePasswordRequest validatePasswordRequest)
    {
        try
        {
            if (validatePasswordRequest == null || string.IsNullOrEmpty(validatePasswordRequest.Password))
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Password is required", 400));
            }

            var validationResult = _passwordValidator.ValidatePassword(validatePasswordRequest.Password);

            var response = new
            {
                isValid = validationResult.IsValid,
                message = validationResult.ErrorMessage,
                requirements = _passwordValidator.GetPasswordRequirements()
            };

            if (validationResult.IsValid)
            {
                _logger.LogInformation("Password validation successful");
                return Ok(ApiResponse<object>.SuccessResponse(response, "Password is valid", 200));
            }
            else
            {
                _logger.LogWarning($"Password validation failed: {validationResult.ErrorMessage}");
                return Ok(ApiResponse<object>.SuccessResponse(response, validationResult.ErrorMessage, 200));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating password");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during password validation", 500));
        }
    }

    /// <summary>
    /// Register new user endpoint (for future implementation with database)
    /// </summary>
    /// <param name="registerRequest">Registration details</param>
    /// <returns>Success or error response</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterRequest registerRequest)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(registerRequest.Username) || string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                _logger.LogWarning("Registration attempt with missing credentials");
                return BadRequest(ApiResponse<object>.ErrorResponse("Username and password are required", 400));
            }

            // Validate username format (basic)
            if (registerRequest.Username.Length < 3 || registerRequest.Username.Length > 50)
            {
                _logger.LogWarning("Registration attempt with invalid username length");
                return BadRequest(ApiResponse<object>.ErrorResponse("Username must be between 3 and 50 characters", 400));
            }

            // Validate password strength
            var passwordValidation = _passwordValidator.ValidatePassword(registerRequest.Password);
            if (!passwordValidation.IsValid)
            {
                _logger.LogWarning($"Registration failed: {passwordValidation.ErrorMessage}");
                return BadRequest(ApiResponse<object>.ErrorResponse($"Password validation failed: {passwordValidation.ErrorMessage}", 400));
            }

            // Check if user already exists (demo implementation)
            if (ValidUsers.ContainsKey(registerRequest.Username))
            {
                _logger.LogWarning($"Registration attempt for existing user: {registerRequest.Username}");
                return BadRequest(ApiResponse<object>.ErrorResponse("Username already exists", 400));
            }

            // Hash password
            var hashedPassword = _passwordHasher.HashPassword(registerRequest.Password);

            // Add user to in-memory store (in production, save to database)
            ValidUsers[registerRequest.Username] = hashedPassword;

            // Generate token for new user
            var token = _tokenService.GenerateToken(registerRequest.Username);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var tokenResponse = new TokenResponse
            {
                Token = token,
                Username = registerRequest.Username,
                ExpiresAt = expiresAt,
                TokenType = "Bearer"
            };

            _logger.LogInformation($"New user registered successfully: {registerRequest.Username}");
            return Created("api/auth/login", ApiResponse<TokenResponse>.SuccessResponse(tokenResponse, "Registration successful", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during registration", 500));
        }
    }
}

