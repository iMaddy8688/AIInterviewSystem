using AIInterview.Shared.DTOs;
using AIInterview.Shared.Models;

namespace AIInterview.API.Services.SecurityService
{
    public interface IStudentRegistration
    {
        Task<StudentRegisterResponce> StudentRegstration(StudentRegisterDto register, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<StudentModel?> ExistsByRollNoAsync(string RollNo, CancellationToken cancellationToken = default);
    }
}
