using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIInterview.Shared.DTOs
{
    public record RefreshTokenDto(string Token);
    public record StudentRegisterResponce(Guid Id,string Email);
    public record StudentRegisterDto(string FullName, string RollNumber, string Password, string Batch,string Email,string ProfilePictureUrl);
    public record LoginDto(string RollNumber, string Password);
    public record AuthResponseDto(string Token, string FullName, string Batch, int StudentId);

    //// Topic Dtops
            public record TopicDto(
            Guid Id,
            string Name,
            string Icon,
            string Description,
            int SortOrder,
            bool IsActive
        );
            public record CreateTopicDto(
               string Name,
               string Icon,
               string Description,
               int SortOrder
           );
            public record UpdateTopicDto(
               Guid Id,
               string Name,
               string Icon,
               string Description,
               int SortOrder,
               bool IsActive
           );

    // ── Question DTOs ──────────────────────────────────────────
    public record QuestionDto(
        Guid QuestionId,
        Guid TopicId,
        string TopicName,
        string Text,
        string Level,
        string? ModelAnswer,
        int TimeLimit,
        bool IsActive
    );
    public record CreateQuestionDto(
           Guid TopicId,
           string Text,
           string Level,
           string? ModelAnswer,
           int TimeLimit
       );

    public record UpdateQuestionDto(
        Guid QuestionId,
        Guid TopicId,
        string Text,
        string Level,
        string? ModelAnswer,
        int TimeLimit,
        bool IsActive
    );
    // ── Student ke liye — ModelAnswer hidden ───────────────────
    public record SessionQuestionDto(
        Guid QuestionId,
        string Text,
        string Level,
        int TimeLimit
    );

    // ── Bulk add ───────────────────────────────────────────────
    public record BulkCreateQuestionDto(
        Guid TopicId,
        string Level,
        List<string> Questions
    );
    // Subject Category DTOs
    public record SubjectCategoryDto(
        int SubjectCategoryId,
        string Name,
        string Icon,
        string Description,
        bool IsActive
    );

    public record CreateSubjectCategoryDto(
        string Name,
        string Icon,
        string Description
    );

    public record UpdateSubjectCategoryDto(
        int SubjectCategoryId,
        string Name,
        string Icon,
        string Description,
        bool IsActive
    );

    // Enrollment DTOs
    public record EnrollSubjectsDto(
        List<int> SubjectCategoryIds  // student jo subjects select kare
    );

    public record MyEnrollmentDto(
        int SubjectCategoryId,
        string Name,
        string Icon,
        string Description,
        DateTime EnrolledAt
    );


}
