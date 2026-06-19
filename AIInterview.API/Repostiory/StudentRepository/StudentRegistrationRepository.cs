using AIInterview.API.Data;
using AIInterview.API.Services.SecurityService;
using AIInterview.API.Services.StudentService;
using AIInterview.Shared.DTOs;
using AIInterview.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AIInterview.API.Repostiory.StudentRepository
{
    public class StudentRegistrationRepository: IStudentRegistration
    {
        private readonly AppDbContext _dbContext;  
        private readonly IPasswordHasher _passwordHasher;
        public StudentRegistrationRepository(AppDbContext _db, IPasswordHasher passwordHasher)
        {
            _dbContext = _db;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Students.AnyAsync(s => s.Email == email, cancellationToken);
        }

        public async Task<StudentRegisterResponce> StudentRegstration(StudentRegisterDto register, CancellationToken cancellationToken = default)
        {
            var emailExists = await ExistsByEmailAsync(register.Email, cancellationToken);

            if (emailExists)
            {
                throw new Exception("Email already exists.");
            }
            var encryptedPassword = _passwordHasher.HashPassword(register.Password);
            var student = new StudentModel(register.RollNumber,register.FullName, encryptedPassword, register.Batch,register.Email);
            await _dbContext.Students.AddAsync(student, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new StudentRegisterResponce(student.StudentId, student.Email);
        }
    }
}
 