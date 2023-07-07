using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace WebApiTelegramBot.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTimeOffset RegisteredAt { get; set; }
        public string Phone { get; set; } = null!;
        public StudentStatus StudentStatus { get; set; }
        public long? ApplicationUserId { get; set; } = null;
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
    public enum StudentStatus
    {
        Active = 1,
        InActive = 2
    }
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTimeOffset RegisteredAt { get; set; }
        public string Phone { get; set; } = null!;
        public StudentStatus StudentStatus { get; set; }
        public long? ApplicationUserId { get; set; } = null;
    }
    public class GetStudentByUserIdDto
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTimeOffset RegisteredAt { get; set; }
        public string Phone { get; set; } = null!;
        public StudentStatus StudentStatus { get; set; }
        public long? ApplicationUserId { get; set; } = null;
        public GetStudentGroupDto StudentGroupDtos { get; set; }
    }
}


