using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace WebApiTelegramBot.Entities
{
    public class ProgressBook
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int TimeTableId { get; set; }
        public virtual TimeTable TimeTable { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public bool IsAttended { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }
        public int LateMinutes { get; set; }
    }
    public class ProgressBookDto
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int TimeTableId { get; set; }
        public int StudentId { get; set; }
        public bool IsAttended { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        public int GroupId { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }
        public int LateMinutes { get; set; }
    }
}
