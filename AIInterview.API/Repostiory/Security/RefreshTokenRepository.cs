using AIInterview.API.Data;
using AIInterview.API.Services.SecurityService;
using AIInterview.Shared.Models.SecurityModel;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.Security
{
    public class RefreshTokenRepository(AppDbContext db) : IRefreshToken
    {
        public async Task<StudentRefreshTokenModel?> CreateAsync(Guid studentId, string token, int days = 7)
        {
            var refreshToken = new StudentRefreshTokenModel
            {
                StudentId = studentId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(days),
                IsRevoked = false
            };
            db.StudentRefreshTokens.Add(refreshToken);
            await db.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<StudentRefreshTokenModel?> GetByTokenAsync(string token)
        {
           return await db.StudentRefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);   
        }

        public async Task RevokeAllForStudentAsync(Guid studentId)
        {
            var tokens = await db.StudentRefreshTokens.Where(rt => rt.StudentId == studentId && !rt.IsRevoked).ToListAsync();
        }

        public async Task RevokeAsync(string token, string? replacedBy = null)
        {
            var rt = await db.StudentRefreshTokens
                         .FirstOrDefaultAsync(r => r.Token == token);
            if (rt is null) return;

            rt.IsRevoked = true;
            rt.ReplacedByToken = replacedBy;
            await db.SaveChangesAsync();
        }
    }
}
