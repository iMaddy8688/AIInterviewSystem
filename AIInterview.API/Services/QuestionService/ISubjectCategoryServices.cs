using AIInterview.Shared.Models;

namespace AIInterview.API.Services.QuestionService
{
    public interface ISubjectCategoryServices
    {
        Task<List<SubjectCategoryModel>> GetAllAsync(bool activeOnly = true);
        Task<SubjectCategoryModel?> GetByIdAsync(int id);
        Task<SubjectCategoryModel> CreateAsync(SubjectCategoryModel model);
        Task<SubjectCategoryModel?> UpdateAsync(SubjectCategoryModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string name, int? excludeId = null);
    }
}
