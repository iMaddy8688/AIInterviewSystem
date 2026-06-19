using AIInterview.Shared.DTOs;

namespace AIInterview.API.Services.SecurityService
{
    public interface IStudentRegistration
    {
        Task<StudentRegisterResponce> StudentRegstration(StudentRegisterDto register, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
