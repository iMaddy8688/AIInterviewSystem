using AIInterview.API.Data;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.QuestionRepository
{
    public class QuestionRepository(AppDbContext db) : IQuestionServices
    {
        public Task<int> CountAsync(Guid topicId, string level)
               => db.Question.CountAsync(q =>
                        q.TopicId == topicId &&
                        q.Level == level &&
                        (q.IsActive ?? false));

        public async Task<QuestionModels> CreateAsync(QuestionModels q)
        {
            q.QuestionId = Guid.NewGuid();
            q.CreatedAt = DateTime.UtcNow;
            db.Question.Add(q);
            await db.SaveChangesAsync();
            return q;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var q = await db.Question.FindAsync(id);
            if (q is null) return false;
            // Soft delete
            q.IsActive = false;
            await db.SaveChangesAsync();
            return true;
        }

        public Task<List<QuestionModels>> GetAllAsync(Guid? topicId, string? level, bool? isActive)
        {
            var q = db.Question.Include(x => x.Topic).AsQueryable();
            if (topicId.HasValue) q = q.Where(x => x.TopicId == topicId);
            if (!string.IsNullOrEmpty(level)) q = q.Where(x => x.Level == level);
            if (isActive.HasValue) q = q.Where(x => x.IsActive == isActive);
            return q.OrderBy(x => x.TopicId)
                    .ThenBy(x => x.Level)
                    .ToListAsync();
        }

        public Task<QuestionModels?> GetByIdAsync(Guid id)
            => db.Question
                 .Include(q => q.Topic)
                 .FirstOrDefaultAsync(q => q.QuestionId == id);
        

         public Task<List<QuestionModels>> GetRandomForSessionAsync(Guid topicId, string level, int count = 5)
                     => db.Question
                    .Where(q => q.TopicId == topicId
                             && q.Level == level
                             && q.IsActive == true)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(count)
                    .ToListAsync();

        public async Task<QuestionModels?> UpdateAsync(QuestionModels q)
        {
            var existing = await db.Question.FindAsync(q.QuestionId);
            if (existing is null) return null;

            existing.TopicId = q.TopicId;
            existing.Text = q.Text;
            existing.Level = q.Level;
            existing.ModelAnswer = q.ModelAnswer;
            existing.TimeLimit = q.TimeLimit;
            existing.IsActive = q.IsActive;

            await db.SaveChangesAsync();
            return existing;
        }
    }
}
