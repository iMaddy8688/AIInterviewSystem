using AIInterview.Shared.Models;

namespace AIInterview.API.Services.QuestionService
{
    public interface IStudentEnrollmentService
    {
        // Student ke enrolled subjects
        Task<List<StudentSubjectEnrollment>> GetByStudentAsync(Guid studentId);

        // Enroll karo — pehle wale remove karke naye add karo
        Task EnrollAsync(Guid studentId, List<int> subjectCategoryIds);

        // Check karo — student ne koi subject enroll kiya hai?
        Task<bool> HasEnrollmentAsync(Guid studentId);
    }
}
