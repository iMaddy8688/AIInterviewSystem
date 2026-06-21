using AIInterview.Shared.Models.SecurityModel;

namespace AIInterview.API.Services.SecurityService
{
    public interface IRefreshToken
    {
        Task<StudentRefreshTokenModel?> CreateAsync(Guid studentId, string token, int days = 7);
        Task<StudentRefreshTokenModel?> GetByTokenAsync(string token);
        Task RevokeAsync(string token, string? replacedBy = null);
        Task RevokeAllForStudentAsync(Guid studentId);
    }
}
