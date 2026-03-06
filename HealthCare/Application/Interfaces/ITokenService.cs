using System.Security.Claims;

namespace HealthCare.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username);
    ClaimsPrincipal? ValidateToken(string token);
}
