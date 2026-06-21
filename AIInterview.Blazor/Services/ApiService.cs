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
        // ── Student: Get my profile ───────────────────────────────
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
        // ── Topics list ───────────────────────────────────────────
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
    }
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
  DateTime CompletedAt);
    public record QuestionCountDto(int Count);

}

