namespace WebApiTelegramBot.Entities
{
    public class TimeTable
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public TimeTableType TimeTableType { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
    public enum TimeTableType
    {
        Lecture,
        Practice,
        Exam
    }
    public class TimeTableDto
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public TimeTableType TimeTableType { get; set; }
        public int GroupId { get; set; }
    }
}
