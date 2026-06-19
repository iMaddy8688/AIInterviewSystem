using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.DTOs
{
    public record StudentRegisterResponce(Guid Id,string Email);
    public record StudentRegisterDto(string FullName, string RollNumber, string Password, string Batch,string Email,string ProfilePictureUrl);
    public record LoginDto(string RollNumber, string Password);
    public record AuthResponseDto(string Token, string FullName, string Batch, int StudentId);

}
