using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class InterviewSessionModels
    {
        public Guid InterviewSessionId { get; set; }
        public Guid StudentId { get; set; }
        public StudentModel Student { get; set; } = null!;
        public string RoundType { get; set; } = "";
        public int TotalScore { get; set; }
        public bool CheatFlagged { get; set; }
        public int CheatEventCount { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public ICollection<StudentAnswerModel> Answers { get; set; } = [];
        public ICollection<CheatEventModel> CheatEvents { get; set; } = [];
    }
}
