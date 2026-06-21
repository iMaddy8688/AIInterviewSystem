using AIInterview.API.Services.SecurityService;
using AIInterview.Shared.Models.SecurityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
namespace AIInterview.API.Repostiory.Security
{
    public class JwtServiceRepository(IConfiguration config) : IJwtService
    {
        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateToken(Guid studentId, string rollNumber, string fullName, string email)
        {
            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim("StudentId", studentId.ToString()),
                new System.Security.Claims.Claim("RollNumber", rollNumber),
                new System.Security.Claims.Claim("FullName", fullName)
            };
             var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateRefreshToken(StudentRefreshTokenModel token)
        {
            return !token.IsRevoked
                && token.ExpiresAt > DateTime.UtcNow;
        }
    }
}
