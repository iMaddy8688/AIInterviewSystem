using AIInterview.Shared.Models;

namespace AIInterview.API.Services.QuestionService
{
    public interface IQuestionServices
    {
        Task<List<QuestionModels>> GetAllAsync(Guid? topicId, string? level, bool? isActive);
        Task<QuestionModels?> GetByIdAsync(Guid id);
        Task<QuestionModels> CreateAsync(QuestionModels q);
        Task<QuestionModels?> UpdateAsync(QuestionModels q);
        Task<bool> DeleteAsync(Guid id);
        Task<int> CountAsync(Guid topicId, string level);
        Task<List<QuestionModels>> GetRandomForSessionAsync(Guid topicId, string level, int count = 5);
    }
}
