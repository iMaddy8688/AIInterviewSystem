using AIInterview.API.Repostiory.QuestionRepository;
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
    public class SubjectCategoryController(ISubjectCategoryServices repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<SubjectCategoryDto>>> GetAll()
        {
            var list = await repo.GetAllAsync(activeOnly: true);
            return Ok(list.Select(MapDto));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubjectCategoryDto>> GetById(int id)
        {
            var item = await repo.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(MapDto(item));
        }
        // POST api/subjectcategory — Admin add kare
        [HttpPost]
        public async Task<ActionResult<SubjectCategoryDto>> Create(
            [FromBody] CreateSubjectCategoryDto dto)
        {
            if (await repo.ExistsAsync(dto.Name))
                return Conflict(new { message = $"'{dto.Name}' already exists." });

            var model = new SubjectCategoryModel
            {
                Name = dto.Name.Trim(),
                Icon = dto.Icon.Trim(),
                Description = dto.Description.Trim(),
                IsActive = true
            };

            var created = await repo.CreateAsync(model);
            return CreatedAtAction(nameof(GetById),
                new { id = created.SubjectCategoryId }, MapDto(created));
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubjectCategoryDto>> Update(
           int id, [FromBody] UpdateSubjectCategoryDto dto)
        {
            if (id != dto.SubjectCategoryId)
                return BadRequest(new { message = "ID mismatch." });

            if (await repo.ExistsAsync(dto.Name, excludeId: id))
                return Conflict(new { message = $"'{dto.Name}' already exists." });

            var model = new SubjectCategoryModel
            {
                SubjectCategoryId = dto.SubjectCategoryId,
                Name = dto.Name.Trim(),
                Icon = dto.Icon.Trim(),
                Description = dto.Description.Trim(),
                IsActive = dto.IsActive
            };

            var updated = await repo.UpdateAsync(model);
            return updated is null ? NotFound() : Ok(MapDto(updated));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await repo.DeleteAsync(id);
            if (!deleted)
                return BadRequest(new
                {
                    message = "Delete nahi hua. Students enrolled hain is subject mein."
                });
            return NoContent();
        }

        private static SubjectCategoryDto MapDto(SubjectCategoryModel s) =>
      new(s.SubjectCategoryId, s.Name, s.Icon, s.Description, s.IsActive);
    }
}
