using AIInterview.API.Repostiory.Security;
using AIInterview.API.Services.SecurityService;
using AIInterview.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIInterview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IStudentRegistration studentRepo, IJwtService jwt, IRefreshToken refreshRepo) : ControllerBase

    {
        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var student = await studentRepo.ExistsByRollNoAsync(dto.RollNumber);
            if (student is null)
                return Unauthorized(new { message = "Student Roll number not found." });

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash);
            if (!isValidPassword)
                return Unauthorized(new { message = "Incorrect password." });

            if (!student.IsActive)
                return Unauthorized(new { message = "Account InActive." });

            //JWT token generate 
            var token = jwt.GenerateToken(
                   student.StudentId,
                   student.RollNumber,
                   student.FullName,
                   student.Email
                );
            var refreshTokenStr = jwt.GenerateRefreshToken();
            await refreshRepo.CreateAsync(student.StudentId, refreshTokenStr, days: 7);
            return Ok(new
            {
                token,
                student.StudentId,
                student.RollNumber,
                student.FullName,
                student.Batch,
                accessTokenExpires = DateTime.UtcNow.AddHours(1), 
                refreshTokenExpires = DateTime.UtcNow.AddDays(7)
            });





        }
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var studentId = User.FindFirst("StudentId")?.Value;
            var rollNumber = User.FindFirst("RollNumber")?.Value;
            var fullName = User.FindFirst("FullName")?.Value;

            return Ok(new { studentId, rollNumber, fullName });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var storedToken = await refreshRepo.GetByTokenAsync(dto.Token);
            if (storedToken is null)
                return Unauthorized(new { message = "Invalid refresh token." });

            if (!jwt.ValidateRefreshToken(storedToken))
                return Unauthorized(new { message = "Refresh token expired." });
            var student = storedToken.Student;
            var newAccessToken = jwt.GenerateToken(
                                student.StudentId,
                                student.RollNumber,
                                student.FullName,
                                student.Email
                                );
            var newRefreshToken = jwt.GenerateRefreshToken();
            await refreshRepo.RevokeAsync(storedToken.Token, replacedBy: newRefreshToken);
            await refreshRepo.CreateAsync(student.StudentId, newRefreshToken, days: 7);
            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken,
                accessTokenExpires = DateTime.UtcNow.AddHours(8),
                refreshTokenExpires = DateTime.UtcNow.AddDays(7)
            });

        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            await refreshRepo.RevokeAsync(dto.Token);
            return Ok(new { message = "Logout successful." });
        }
        [HttpPost("logout-all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            var studentId = Guid.Parse(User.FindFirst("StudentId")!.Value);
            await refreshRepo.RevokeAllForStudentAsync(studentId);
            return Ok(new { message = "Logout for all devices." });
        }
    }
 }
