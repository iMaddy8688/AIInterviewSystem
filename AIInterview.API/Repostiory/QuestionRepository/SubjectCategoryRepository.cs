using AIInterview.API.Data;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.QuestionRepository
{
    public class SubjectCategoryRepository(AppDbContext db) : ISubjectCategoryServices
    {
        public async Task<SubjectCategoryModel> CreateAsync(SubjectCategoryModel model)
        {
            db.SubjectCategories.Add(model);
            await db.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Check karo — koi student enrolled hai?
            var hasEnrollments = await db.SubjectEnrollments
                .AnyAsync(e => e.SubjectCategoryId == id);
            if (hasEnrollments) return false;

            var subject = await db.SubjectCategories.FindAsync(id);
            if (subject is null) return false;

            db.SubjectCategories.Remove(subject);
            await db.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(string name, int? excludeId = null)
        {
            var q = db.SubjectCategories.Where(s => s.Name == name);
            if (excludeId.HasValue)
                q = q.Where(s => s.SubjectCategoryId != excludeId.Value);
            return q.AnyAsync();
        }

        public Task<List<SubjectCategoryModel>> GetAllAsync(bool activeOnly = true)
        {
            var q = db.SubjectCategories.AsQueryable();
            if (activeOnly) q = q.Where(s => s.IsActive);
            return q.OrderBy(s => s.Name).ToListAsync();
        }

        public Task<SubjectCategoryModel?> GetByIdAsync(int id)
         => db.SubjectCategories.FirstOrDefaultAsync(s => s.SubjectCategoryId == id);

        public async Task<SubjectCategoryModel?> UpdateAsync(SubjectCategoryModel model)
        {
            var existing = await db.SubjectCategories
            .FindAsync(model.SubjectCategoryId);
            if (existing is null) return null;

            existing.Name = model.Name;
            existing.Icon = model.Icon;
            existing.Description = model.Description;
            existing.IsActive = model.IsActive;

            await db.SaveChangesAsync();
            return existing;
        }
    }
}
