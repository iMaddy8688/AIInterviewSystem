using AIInterview.Shared.Models;
using AIInterview.Shared.Models.SecurityModel;
using Microsoft.EntityFrameworkCore;


namespace AIInterview.API.Data
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {
            
        }
        public DbSet<StudentModel> Students => Set<StudentModel>();
        public DbSet<StudentSubScriptionModel> StudentSubscriptions => Set<StudentSubScriptionModel>();
        public DbSet<InterviewSessionModels> InterviewSessions=> Set<InterviewSessionModels>();
        public DbSet<StudentAnswerModel> StudentAnswer => Set<StudentAnswerModel>();
        public DbSet<CheatEventModel> CheatEvent=> Set<CheatEventModel>();
        public DbSet<QuestionModels> Question=>Set<QuestionModels>();
        public DbSet<StudentRefreshTokenModel> StudentRefreshTokens => Set<StudentRefreshTokenModel>();
        public DbSet<TopicModel> Topic => Set<TopicModel>();
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            // Student Model Configuration
            mb.Entity<StudentModel>().ToTable("Student");
            mb.Entity<StudentModel>().HasKey(s => s.StudentId);
            mb.Entity<StudentModel>().HasIndex(s => s.RollNumber).IsUnique();
            mb.Entity<StudentModel>().HasIndex(s=>s.Email).IsUnique();
            mb.Entity<StudentModel>().HasIndex(x => x.Batch);
            mb.Entity<StudentModel>().HasIndex(x => new { x.IsActive, x.IsDeleted });

            // Student Subscription Model Configuration

            mb.Entity<StudentSubScriptionModel>().ToTable("StudentSubscription");
            mb.Entity<StudentSubScriptionModel>().HasKey(s => s.SubscriptionId);
            mb.Entity<StudentSubScriptionModel>().HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Interview Session Model Configuration

            mb.Entity<InterviewSessionModels>().ToTable("InterviewSession");
            mb.Entity<InterviewSessionModels>().HasKey(s => s.InterviewSessionId);
            mb.Entity<InterviewSessionModels>().HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<InterviewSessionModels>().HasIndex(x => new { x.StudentId, x.RoundType });

            // Student Answer Model Configuration
            mb.Entity<StudentAnswerModel>().ToTable("StudentAnswer");
            mb.Entity<StudentAnswerModel>().HasKey(s=> s.StudentAnswerId);

            mb.Entity<StudentAnswerModel>().HasOne(s => s.Session)
                .WithMany()
                .HasForeignKey(s => s.InterviewSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cheat Event Model Configuration
            mb.Entity<CheatEventModel>().ToTable("CheatEvent");
            mb.Entity<CheatEventModel>().HasKey(s => s.CheatEventId);
            mb.Entity<CheatEventModel>().HasOne(s => s.Session)
                .WithMany()
                .HasForeignKey(s => s.InterviewSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question Model Configuration
            mb.Entity<QuestionModels>().ToTable("Question");
            mb.Entity<QuestionModels>().HasKey(s => s.QuestionId);

            // Student Refresh Token Model Configuration
            mb.Entity<StudentRefreshTokenModel>().ToTable("StudentRefreshToken");
            mb.Entity<StudentRefreshTokenModel>().HasKey(r => r.RefreshTokenId);
            mb.Entity<StudentRefreshTokenModel>()
                .HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<StudentRefreshTokenModel>()
                .HasIndex(r => r.Token)
                .IsUnique();
            mb.Entity<StudentRefreshTokenModel>()
                .HasIndex(r => new { r.StudentId, r.IsRevoked });

            // Topic
            mb.Entity<TopicModel>().ToTable("Topic");
            mb.Entity<TopicModel>().HasKey(t => t.Id);
            mb.Entity<TopicModel>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }

    }
}
