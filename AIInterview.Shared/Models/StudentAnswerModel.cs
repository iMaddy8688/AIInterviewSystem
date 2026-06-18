using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class StudentAnswerModel
    {
        public Guid StudentAnswerId { get; set; }
        public Guid InterviewSessionId { get; set; }
        public InterviewSessionModels Session { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public QuestionModels Question { get; set; } = null!;
        public string AnswerText { get; set; } = "";
        public int Score { get; set; }
        public string Verdict { get; set; } = "";
        public string Feedback { get; set; } = "";
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow; 
    }
}
