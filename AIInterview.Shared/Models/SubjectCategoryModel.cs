using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class SubjectCategoryModel
    {
        public int SubjectCategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}
