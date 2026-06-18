using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.DTOs
{
    public record LoginDto(string RollNumber, string Password);
    public record AuthResponseDto(string Token, string FullName, string Batch, int StudentId);

}
