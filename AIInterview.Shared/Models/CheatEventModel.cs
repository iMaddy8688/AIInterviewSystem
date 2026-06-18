using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class CheatEventModel
    {
        public Guid CheatEventId { get; set; }
        public Guid InterviewSessionId { get; set; }
        public InterviewSessionModels Session { get; set; } = null!;
        public string EventType { get; set; } = "";
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
