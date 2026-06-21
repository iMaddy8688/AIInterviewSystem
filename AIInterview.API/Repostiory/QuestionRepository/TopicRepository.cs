using AIInterview.API.Data;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.QuestionRepository
{
    public class TopicRepository(AppDbContext db) : ITopicService
    {
        public async Task<TopicModel> CreateAsync(TopicModel topic)
        {
            db.Topic.Add(topic);
            await db.SaveChangesAsync();
            return topic;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            
            var hasQ = await db.Question.AnyAsync(q => q.TopicId == id);
            if (hasQ) return false;

            var topic = await db.Topic.FindAsync(id);
            if (topic is null) return false;

            db.Topic.Remove(topic);
            await db.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(string name, Guid? excludeId = null)
        {
            var q = db.Topic.Where(t => t.Name == name);
            if (excludeId.HasValue)
                q = q.Where(t => t.Id != excludeId.Value);
            return q.AnyAsync();
        }

        public  Task<List<TopicModel>> GetAllAsync(bool activeOnly = true)
        {
            var q = db.Topic.AsQueryable();

            if (activeOnly) q = q.Where(t => t.IsActive);
            return q.OrderBy(t => t.SortOrder)
                    .ThenBy(t => t.Name)
                    .ToListAsync();
        }

        public Task<TopicModel?> GetByIdAsync(Guid id)
          => db.Topic.FirstOrDefaultAsync(t => t.Id == id);


        public async Task<TopicModel?> UpdateAsync(TopicModel topic)
        {
            var existing = await db.Topic.FindAsync(topic.Id);
            if (existing is null) return null;

            existing.Name = topic.Name;
            existing.Icon = topic.Icon;
            existing.Description = topic.Description;
            existing.SortOrder = topic.SortOrder;
            existing.IsActive = topic.IsActive;

            await db.SaveChangesAsync();
            return existing;
        }
    }
}
