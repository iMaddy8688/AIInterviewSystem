using AIInterview.Shared.DTOs;
using System.Net.Http.Json;

namespace AIInterview.Blazor.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public ApiService(HttpClient http, AuthService auth)
        {
            _http = http;
            _auth = auth;
        }

        private void SetAuth() => _auth.SetAuthHeader();

        // ── Profile ───────────────────────────────────────────────
        public async Task<StudentProfileDto?> GetMyProfileAsync()
        {
            SetAuth();
            try
            {
                return await _http.GetFromJsonAsync<StudentProfileDto>(
                    "api/auth/me");
            }
            catch { return null; }
        }

        // ── Interview History ─────────────────────────────────────
        public async Task<List<SessionHistoryDto>> GetMyHistoryAsync()
        {
            SetAuth();
            try
            {
                return await _http.GetFromJsonAsync<List<SessionHistoryDto>>(
                    "api/interview/history") ?? [];
            }
            catch { return []; }
        }

        // ── Topics ────────────────────────────────────────────────
        public async Task<List<TopicDto>> GetTopicsAsync()
        {
            SetAuth();
            try
            {
                return await _http.GetFromJsonAsync<List<TopicDto>>(
                    "api/topic") ?? [];
            }
            catch { return []; }
        }

        // ── Question Count ────────────────────────────────────────
        public async Task<int> GetQuestionCountAsync(int topicId, string level)
        {
            SetAuth();
            try
            {
                var res = await _http.GetFromJsonAsync<QuestionCountDto>(
                    $"api/question/count?topicId={topicId}&level={level}");
                return res?.Count ?? 0;
            }
            catch { return 0; }
        }

        // ── Subject Categories ────────────────────────────────────
        public async Task<List<SubjectCategoryDto>> GetSubjectCategoriesAsync()
        {
            SetAuth();
            try
            {
                return await _http.GetFromJsonAsync<List<SubjectCategoryDto>>(
                    "api/subjectcategory") ?? [];
            }
            catch { return []; }
        }

        // ── My Enrolled Subjects ──────────────────────────────────
        public async Task<List<MyEnrollmentDto>> GetMySubjectsAsync()
        {
            SetAuth();
            try
            {
                return await _http.GetFromJsonAsync<List<MyEnrollmentDto>>(
                    "api/student/enrollment/my-subjects") ?? [];
            }
            catch { return []; }
        }

        // ── Enroll Subjects ───────────────────────────────────────
        public async Task<bool> EnrollSubjectsAsync(List<int> subjectIds)
        {
            SetAuth();
            try
            {
                var res = await _http.PostAsJsonAsync(
                    "api/student/enrollment/enroll",
                    new { subjectCategoryIds = subjectIds });
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── Has Enrollment Check ──────────────────────────────────
        public async Task<bool> HasEnrollmentAsync()
        {
            SetAuth();
            try
            {
                var res = await _http.GetFromJsonAsync<HasEnrollmentResponse>(
                    "student/enrollment/has-enrollment");
                return res?.HasEnrollment ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HasEnrollment error: {ex.Message}");
                return false;
            }
        }
    }

    // ── DTOs ──────────────────────────────────────────────────────
    public record StudentProfileDto(
        string StudentId,
        string RollNumber,
        string FullName
    );

    public record SessionHistoryDto(
        Guid SessionId,
        string RoundType,
        string Level,
        int TotalScore,
        bool CheatFlagged,
        DateTime CompletedAt
    );

    public record QuestionCountDto(int Count);

    // Internal response models
    file record HasEnrollmentResponse(bool HasEnrollment);
}