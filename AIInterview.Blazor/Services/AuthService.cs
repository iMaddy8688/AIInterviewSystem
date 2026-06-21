using System.Net.Http.Json;

namespace AIInterview.Blazor.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? FullName { get; private set; }
    public string? RollNumber { get; private set; }
    public string? StudentId { get; private set; }
    public string? Batch { get; private set; }
    public string? Role { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);

    public event Action? OnAuthChanged;

    public AuthService(HttpClient http) => _http = http;

    // ── Student Login ─────────────────────────────────────────
    public async Task<(bool Success, string? Error)> StudentLoginAsync(
        string rollNumber, string password)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/login",
                new { rollNumber, password });

            if (!res.IsSuccessStatusCode)
                return (false, "Invalid roll number or password.");

            var data = await res.Content
                .ReadFromJsonAsync<LoginResponse>();

            if (data is null)
                return (false, "Server error. Try again.");

            // API returns "token" field — not "accessToken"
            AccessToken = data.Token;
            RefreshToken = null;        // not in response yet
            FullName = data.FullName;
            RollNumber = data.RollNumber;
            StudentId = data.StudentId;
            Batch = data.Batch;
            Role = "Student";

            SetAuthHeader();
            OnAuthChanged?.Invoke();
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Cannot connect to server. {ex.Message}");
        }
    }

    // ── Admin Login ───────────────────────────────────────────
    public async Task<(bool Success, string? Error)> AdminLoginAsync(
        string email, string password)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/login",
                new { rollNumber = email, password });

            if (!res.IsSuccessStatusCode)
                return (false, "Invalid credentials.");

            var data = await res.Content
                .ReadFromJsonAsync<LoginResponse>();

            if (data is null)
                return (false, "Server error.");

            AccessToken = data.Token;
            FullName = data.FullName;
            RollNumber = data.RollNumber;
            StudentId = data.StudentId;
            Batch = data.Batch;
            Role = "Admin";

            SetAuthHeader();
            OnAuthChanged?.Invoke();
            return (true, null);
        }
        catch
        {
            return (false, "Cannot connect to server.");
        }
    }

    // ── Helpers ───────────────────────────────────────────────
    public void SetAuthHeader()
    {
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", AccessToken);
    }

    public void Logout()
    {
        AccessToken = null;
        RefreshToken = null;
        FullName = null;
        RollNumber = null;
        StudentId = null;
        Batch = null;
        Role = null;
        _http.DefaultRequestHeaders.Authorization = null;
        OnAuthChanged?.Invoke();
    }

    public string Initials => string.IsNullOrEmpty(FullName) ? "?" :
        string.Join("", FullName.Split(' ')
            .Take(2).Select(w => w[0]));

    // ── Response model matching API exactly ───────────────────
    private record LoginResponse(
        string? Token,
        string? StudentId,
        string? RollNumber,
        string? FullName,
        string? Batch,
        DateTime? AccessTokenExpires,
        DateTime? RefreshTokenExpires
    );
}