using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models.SecurityModel
{
    public class StudentRefreshTokenModel
    {
        public Guid RefreshTokenId { get; set; }
        public Guid StudentId { get; set; }   // aapka StudentId Guid hai
        public string Token { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;
        public string? ReplacedByToken { get; set; }  // rotation ke liye
        public StudentModel Student { get; set; } = null!;
    }
}
