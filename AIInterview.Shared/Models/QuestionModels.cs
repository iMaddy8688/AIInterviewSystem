using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class QuestionModels
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; } = "";
        public string Level { get; set; } = "Intermediate";
        public Guid TopicId { get; set; }

        public TopicModel Topic { get; set; } = null!;

        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? ModelAnswer { get; set; }
        public int? TimeLimit { get; set; }
    }
}
