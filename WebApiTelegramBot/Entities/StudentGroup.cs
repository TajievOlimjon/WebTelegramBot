using System.ComponentModel.DataAnnotations;

namespace WebApiTelegramBot.Entities
{
    public class StudentGroup
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public StudentGroupStatus StudentGroupStatus { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
    public enum StudentGroupStatus
    {
        Active=1,
        Finished=2,
        Left=3,
    }
    public class AddStudentGroupDto
    {
        public int GroupId { get; set; }
        public int StudentId { get; set; }
        public StudentGroupStatus StudentGroupStatus { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
    public class UpdateStudentGroupDto
    {
        public int GroupId { get; set; }
        public int StudentId { get; set; }
        public int NewGroupId { get; set; }
        public StudentGroupStatus StudentGroupStatus { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
    public class GetStudentGroupDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int StudentId { get; set; }
        public StudentGroupStatus StudentGroupStatus { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
}
