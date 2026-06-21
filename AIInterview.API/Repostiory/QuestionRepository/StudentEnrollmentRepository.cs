using AIInterview.API.Data;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.QuestionRepository
{
    public class StudentEnrollmentRepository(AppDbContext db) : IStudentEnrollmentService
    {
        public async Task EnrollAsync(Guid studentId, List<int> subjectCategoryIds)
        {
            // Pehle wale sab deactivate karo
            var existing = await db.SubjectEnrollments
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
            db.SubjectEnrollments.RemoveRange(existing);

            // Naye add karo
            foreach (var id in subjectCategoryIds.Distinct())
            {
                db.SubjectEnrollments.Add(new StudentSubjectEnrollment
                {
                    StudentId = studentId,
                    SubjectCategoryId = id,
                    EnrolledAt = DateTime.UtcNow,
                    IsActive = true
                });
            }

            await db.SaveChangesAsync();
        }

        public Task<List<StudentSubjectEnrollment>> GetByStudentAsync(Guid studentId)
        => db.SubjectEnrollments
             .Include(e => e.SubjectCategory)
             .Where(e => e.StudentId == studentId && e.IsActive)
             .OrderBy(e => e.SubjectCategory.Name)
             .ToListAsync();

        public Task<bool> HasEnrollmentAsync(Guid studentId)
       => db.SubjectEnrollments
             .AnyAsync(e => e.StudentId == studentId && e.IsActive);
    }
}
