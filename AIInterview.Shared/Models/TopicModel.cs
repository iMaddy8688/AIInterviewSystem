using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class TopicModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Description { get; set; } = "";
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public ICollection<QuestionModels> Questions { get; set; } = [];
    }
}
