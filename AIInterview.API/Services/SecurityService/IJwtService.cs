using AIInterview.Shared.Models.SecurityModel;

namespace AIInterview.API.Services.SecurityService
{
    public interface IJwtService
    {

        string GenerateToken(Guid studentId, string rollNumber, string fullName,string email);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(StudentRefreshTokenModel token);
    }
}
