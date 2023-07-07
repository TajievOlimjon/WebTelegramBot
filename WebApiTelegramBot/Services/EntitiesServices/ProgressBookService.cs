using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services.EntitiesServices
{
    public class ProgressBookService
    {
        public IEnumerable<ProgressBookDto> GetAllProgressbooksAsync(int groupId,int studentId)
        {

            var progressors = new List<ProgressBookDto>()
            {
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,03), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Monday
                },
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,04), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Tuesday
                },
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,05), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Wednesday
                },
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,06), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Thursday
                },
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,07), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Friday
                },
                new ProgressBookDto
                {
                    Date = DateTime.SpecifyKind(new DateTime(2023,07,08), DateTimeKind.Utc),
                    Grade=15,
                    GroupId=groupId,
                    IsAttended=true,
                    LateMinutes=0,
                    StudentId=studentId,
                    TimeTableId=(int)DayOfWeek.Saturday
                }
            };
            return progressors;
        }
    }
}

