namespace WebApiTelegramBot.Entities
{
    public class Group
    {
        public int GroupId { get; set; }
        public CourseFormat CourseFormat { get; set; }
        public string GroupName { get; set; }
        public DurationType DurationType { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
    public enum DurationType
    {
        Days,
        Weeks,
        Months,
        Years
    }

    public enum GroupStatus
    {
        Preparing,
        Started,
        Finished,
    }

    public enum CourseFormat
    {
        Online,
        Offline,
        Workshop
    }
    public class GroupDto
    {
        public int GroupId { get; set; }
        public CourseFormat CourseFormat { get; set; }
        public string GroupName { get; set; }
        public DurationType DurationType { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
        public GroupStatus GroupStatus { get; set; }
    }
}

