using AIInterview.Shared.Models;

namespace AIInterview.API.Services.QuestionService
{
    public interface ITopicService
    {
        Task<List<TopicModel>> GetAllAsync(bool activeOnly = true);
        Task<TopicModel?> GetByIdAsync(Guid id);
        Task<TopicModel> CreateAsync(TopicModel topic);
        Task<TopicModel?> UpdateAsync(TopicModel topic);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string name, Guid? excludeId = null);
    }
}
