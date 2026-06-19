using AIInterview.API.Services.SecurityService;
using AIInterview.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIInterview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRegistrationController : ControllerBase
    {
        IStudentRegistration studentRegistration;
        public StudentRegistrationController(IStudentRegistration registration)
        {
            studentRegistration = registration;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterDto register, CancellationToken cancellationToken)
        {
            try
            {
                var result = await studentRegistration.StudentRegstration(register, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
