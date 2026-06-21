using AIInterview.API.Services.QuestionService;
using AIInterview.Shared.DTOs;
using AIInterview.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIInterview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TopicController(ITopicService repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<TopicDto>>> GetAll()
        {
            var topics = await repo.GetAllAsync(activeOnly: true);
            return Ok(topics.Select(MapDto));
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<TopicDto>>> GetAllAdmin()
        {
            var topics = await repo.GetAllAsync(activeOnly: false);
            return Ok(topics.Select(MapDto));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TopicDto>> GetById(Guid id)
        {
            var topic = await repo.GetByIdAsync(id);
            return topic is null ? NotFound() : Ok(MapDto(topic));
        }
        [HttpPost]
        public async Task<ActionResult<TopicDto>> Create([FromBody] CreateTopicDto dto)
        {
            if (await repo.ExistsAsync(dto.Name))
                return Conflict(new { message = $"'{dto.Name}' topic already exists." });

            var topic = new TopicModel
            {
                Name = dto.Name.Trim(),
                Icon = dto.Icon.Trim(),
                Description = dto.Description.Trim(),
                SortOrder = dto.SortOrder,
                IsActive = true
            };

            var created = await repo.CreateAsync(topic);
            return CreatedAtAction(nameof(GetById),
                new { id = created.Id }, MapDto(created));
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TopicDto>> Update(
           Guid id, [FromBody] UpdateTopicDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch." });

            if (await repo.ExistsAsync(dto.Name, excludeId: id))
                return Conflict(new { message = $"'{dto.Name}' already exists." });

            var topic = new TopicModel
            {
                Id = dto.Id,
                Name = dto.Name.Trim(),
                Icon = dto.Icon.Trim(),
                Description = dto.Description.Trim(),
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive
            };

            var updated = await repo.UpdateAsync(topic);
            return updated is null ? NotFound() : Ok(MapDto(updated));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await repo.DeleteAsync(id);
            if (!deleted)
                return BadRequest(new
                {
                    message = "Not Deleted."
                });
            return NoContent();
        }
        public static TopicDto MapDto(TopicModel topic)
        {
            return new TopicDto(
                Id: topic.Id,
                Name: topic.Name,
                Icon: topic.Icon,
                Description: topic.Description,
                SortOrder: topic.SortOrder,
                IsActive: topic.IsActive
            );
        }

    }   


}
