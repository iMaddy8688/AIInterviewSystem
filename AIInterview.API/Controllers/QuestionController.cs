using AIInterview.API.Repostiory.QuestionRepository;
using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.DTOs;
using AIInterview.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace AIInterview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionController(IQuestionServices qRepo, ITopicService tRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<QuestionDto>>> GetAll([FromQuery] Guid? topicId = null,[FromQuery] string? level = null,[FromQuery] bool? isActive = true)
        {
            var questions = await qRepo.GetAllAsync(topicId, level, isActive);
            return Ok(questions.Select(MapDto));
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<QuestionDto>> GetById(Guid id)
        {
            var q = await qRepo.GetByIdAsync(id);
            return q is null ? NotFound() : Ok(MapDto(q));
        }
        [HttpGet("count")]
        public async Task<IActionResult> Count(
         [FromQuery] Guid topicId,
         [FromQuery] string level = "Intermediate")
        {
            var count = await qRepo.CountAsync(topicId, level);
            return Ok(new { topicId, level, count });
        }
        [HttpGet("session")]
        public async Task<ActionResult<List<SessionQuestionDto>>> GetForSession(
           [FromQuery] Guid topicId,
           [FromQuery] string level = "Intermediate",
           [FromQuery] int count = 5)
        {
            var questions = await qRepo.GetRandomForSessionAsync(topicId, level, count);

            if (!questions.Any())
                return NotFound(new
                {
                    message = $"No Quation Available — TopicId:{topicId}, Level:{level}",
                    hint = "First Add Quatoins."
                });

            return Ok(questions.Select(q => new SessionQuestionDto(
                  QuestionId:q.QuestionId,
                    Text:q.Text,
                    Level:q.Level,
                    TimeLimit:q.TimeLimit is int timeLimit ? timeLimit : 0

                )));
            
        }
        private static bool IsValidLevel(string level) =>
         level is "Beginner" or "Intermediate" or "Advanced";
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] CreateQuestionDto dto)
        {
            var topic = await tRepo.GetByIdAsync(dto.TopicId);
            if (topic is null)
                return BadRequest(new { message = $"TopicId {dto.TopicId} exist nahi karta." });

            if (!IsValidLevel(dto.Level))
                return BadRequest(new { message = "Level: Beginner, Intermediate, ya Advanced hona chahiye." });

            var q = new QuestionModels
            {
                TopicId = dto.TopicId,
                Text = dto.Text.Trim(),
                Level = dto.Level,
                ModelAnswer = dto.ModelAnswer?.Trim(),
                TimeLimit = dto.TimeLimit > 0 ? dto.TimeLimit : 120,
                IsActive = true
            };

            var created = await qRepo.CreateAsync(q);
            return CreatedAtAction(nameof(GetById),
                new { id = created.QuestionId }, MapDto(created));
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<QuestionDto>> Update(
           Guid id, [FromBody] UpdateQuestionDto dto)
        {
            if (id != dto.QuestionId)
                return BadRequest(new { message = "ID mismatch." });

            var q = new QuestionModels
            {
                QuestionId = dto.QuestionId,
                TopicId = dto.TopicId,
                Text = dto.Text.Trim(),
                Level = dto.Level,
                ModelAnswer = dto.ModelAnswer?.Trim(),
                TimeLimit = dto.TimeLimit,
                IsActive = dto.IsActive
            };

            var updated = await qRepo.UpdateAsync(q);
            return updated is null ? NotFound() : Ok(MapDto(updated));
        }

        // DELETE api/question/{id}  (soft delete)
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await qRepo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        private static QuestionDto MapDto(QuestionModels q)
        {
            return new QuestionDto(
           QuestionId: q.QuestionId,
           TopicId: q.TopicId,
           TopicName:q.Topic?.Name,
           Text:q.Text,
           Level:q.Level,
           ModelAnswer: q.ModelAnswer is string modelAnswer ? modelAnswer : q.ModelAnswer?.ToString() ?? string.Empty,
           TimeLimit: q.TimeLimit is int timeLimit ? timeLimit : 0,
          IsActive: q.IsActive ?? false

       );

        }
            
    }
}
