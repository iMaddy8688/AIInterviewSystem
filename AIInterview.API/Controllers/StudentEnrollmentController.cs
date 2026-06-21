using AIInterview.API.Repostiory.QuestionRepository;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIInterview.API.Controllers
{
    [ApiController]
    [Route("api/student/enrollment")]
    [Authorize]
    public class StudentEnrollmentController(IStudentEnrollmentService enrollRepo,
    ISubjectCategoryServices subjectRepo) : ControllerBase
    {
        [HttpGet("my-subjects")]
        public async Task<ActionResult<List<MyEnrollmentDto>>> GetMySubjects()
        {
            var studentId = GetStudentId();
            var enrollments = await enrollRepo.GetByStudentAsync(studentId);

            // HR hamesha show hoga — sab students ke liye
            var result = new List<MyEnrollmentDto>
        {
            new(0, "HR", "🤝", "Behavioral & communication round",
                DateTime.UtcNow)
        };

            // Enrolled subjects add karo
            result.AddRange(enrollments.Select(e => new MyEnrollmentDto(
                e.SubjectCategoryId,
                e.SubjectCategory.Name,
                e.SubjectCategory.Icon,
                e.SubjectCategory.Description,
                e.EnrolledAt
            )));

            return Ok(result);
        }
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollSubjectsDto dto)
        {
            if (dto.SubjectCategoryIds is null || !dto.SubjectCategoryIds.Any())
                return BadRequest(new
                {
                    message = "Kam se kam ek subject select karo."
                });

            if (dto.SubjectCategoryIds.Count > 6)
                return BadRequest(new
                {
                    message = "Max 6 subjects select kar sakte hain."
                });

            // Validate karo — sab IDs exist karti hain?
            foreach (var id in dto.SubjectCategoryIds)
            {
                var subject = await subjectRepo.GetByIdAsync(id);
                if (subject is null || !subject.IsActive)
                    return BadRequest(new
                    {
                        message = $"Subject ID {id} valid nahi hai."
                    });
            }

            var studentId = GetStudentId();
            await enrollRepo.EnrollAsync(studentId, dto.SubjectCategoryIds);

            return Ok(new
            {
                message = "Subjects successfully enrolled!",
                enrolled = dto.SubjectCategoryIds.Count
            });
        }
        [HttpGet("has-enrollment")]
        public async Task<IActionResult> HasEnrollment()
        {
            var studentId = GetStudentId();
            var has = await enrollRepo.HasEnrollmentAsync(studentId);
            return Ok(new { hasEnrollment = has });
        }

        private Guid GetStudentId() =>
            Guid.Parse(User.FindFirst("StudentId")!.Value);

    }
}
