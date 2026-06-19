    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace AIInterview.Shared.Models
    {
        public class StudentModel
        {
            private StudentModel() { }
            public StudentModel(string rollNumber, string fullName, string passwordHash, string batch, string email)
            {
                RollNumber = rollNumber;
                FullName = fullName;
                PasswordHash = passwordHash;
                Batch = batch;
                Email = email;
            }

           public Guid StudentId { get; set; }
            public string RollNumber { get; set; } = "";
            public string FullName { get; set; } = "";
            public string PasswordHash { get; set; } = "";
            public string Batch { get; set; } = "";
            public string Email { get; set; } = "";
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public bool IsActive { get; set; } = true;
            public bool IsDeleted { get; set; }=false;  
            public string ProfilePictureUrl { get; set; } = ""; 
            public ICollection<InterviewSessionModels> Sessions { get; set; } = [];
            //public ICollection<ResumeAnalysis> Resumes { get; set; } = [];
            //public ICollection<LearningRoadmap> Roadmaps { get; set; } = [];
        }
        public class  StudentSubScriptionModel
        {
            public Guid SubscriptionId { get; set; }

            public Guid StudentId { get; set; }
            public StudentModel Student { get; set; } = null!;

            public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
            public DateTime? ExpiredAt { get; set; }

            public bool IsActive { get; set; } = true;

        }
    }
