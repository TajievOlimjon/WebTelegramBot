using Microsoft.EntityFrameworkCore;
using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services.EntitiesServices
{
    public class TimeTableService
    {
        public IEnumerable<TimeTableDto> GetAllTimeTableByGroupIdAsync(int groupId)
        {
            var timetables = new List<TimeTableDto>()
            {
                new TimeTableDto
                {
                    Id = 1,
                    DayOfWeek = DayOfWeek.Monday,
                    TimeTableType = TimeTableType.Lecture,
                    GroupId=groupId
                },
                new TimeTableDto
                {
                    Id = 2,
                    DayOfWeek = DayOfWeek.Tuesday,
                    TimeTableType = TimeTableType.Practice,
                    GroupId=groupId
                },
                new TimeTableDto
                {
                    Id = 3,
                    DayOfWeek = DayOfWeek.Wednesday,
                    TimeTableType = TimeTableType.Lecture,
                    GroupId=groupId
                },
                new TimeTableDto
                {
                    Id = 4,
                    DayOfWeek = DayOfWeek.Thursday,
                    TimeTableType = TimeTableType.Practice,
                    GroupId=groupId
                },
                 new TimeTableDto
                {
                    Id = 5,
                    DayOfWeek = DayOfWeek.Friday,
                    TimeTableType = TimeTableType.Lecture,
                    GroupId=groupId
                },
                  new TimeTableDto
                {
                    Id = 6,
                    DayOfWeek = DayOfWeek.Saturday,
                    TimeTableType = TimeTableType.Practice,
                    GroupId=groupId
                },
                    new TimeTableDto
                {
                    Id = 7,
                    DayOfWeek = DayOfWeek.Sunday,
                    TimeTableType = TimeTableType.Practice,
                    GroupId=groupId
                }
            };
            return timetables;
        }
    }
}
