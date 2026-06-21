using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.Models
{
    public class StudentSubjectEnrollment
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public int SubjectCategoryId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation
        public StudentModel Student { get; set; } = null!;
        public SubjectCategoryModel SubjectCategory { get; set; } = null!;
    }
}
