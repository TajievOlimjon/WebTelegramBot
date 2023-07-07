using Microsoft.EntityFrameworkCore;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services.EntitiesServices
{
    public class StudentService
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _dbContext.Students.Select(student => new StudentDto
            {
              StudentId=student.StudentId,
              FirstName=student.FirstName,
              LastName=student.LastName,
              StudentStatus=student.StudentStatus,
              Phone=student.Phone,
              RegisteredAt=student.RegisteredAt,
              ApplicationUserId=student.ApplicationUserId
            }).ToListAsync();

            return students;
        }
        public async Task<Response<StudentDto>> GetStudentAsync(int studentId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);
            if (student == null) return new Response<StudentDto>(System.Net.HttpStatusCode.NotFound, "Data not found !");

            var studentDto = new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentStatus = student.StudentStatus,
                Phone = student.Phone,
                RegisteredAt = student.RegisteredAt,
                ApplicationUserId = student.ApplicationUserId
            };

            return new Response<StudentDto>(System.Net.HttpStatusCode.NotFound, "Data successfully found !",studentDto);
        }
        public async Task<Response<GetStudentByUserIdDto>> GetStudentByUserIdAsync(long applicationUserId)
        {
            var student = await _dbContext.Students.Where(x=>x.StudentStatus==StudentStatus.Active).FirstOrDefaultAsync(x=>x.ApplicationUserId==applicationUserId);
            if (student == null) return new Response<GetStudentByUserIdDto>(System.Net.HttpStatusCode.NotFound, "Data not found !",null);

            var studentDto = new GetStudentByUserIdDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentStatus = student.StudentStatus,
                Phone = student.Phone,
                RegisteredAt = student.RegisteredAt,
                ApplicationUserId = student.ApplicationUserId,
                StudentGroupDtos = student.StudentGroups.Where(x=>x.Group.GroupStatus==GroupStatus.Started && x.StudentGroupStatus==StudentGroupStatus.Active).Select(x=>new GetStudentGroupDto
                {
                   GroupId=x.GroupId,
                   GroupName=x.Group.GroupName,
                   StartedAt=x.StartedAt,
                   StudentGroupStatus=x.StudentGroupStatus,
                   StudentId=x.StudentId
                }).FirstOrDefault()
            };

            return new Response<GetStudentByUserIdDto>(System.Net.HttpStatusCode.NotFound, "Data successfully found !", studentDto);
        }
        public async Task<Response<StudentDto>> AddStudentAsync(StudentDto student)
        {
            var phone = await _dbContext.Students.FirstOrDefaultAsync(x => x.Phone.Trim() == student.Phone.Trim());
            if (phone != null) return new Response<StudentDto>(System.Net.HttpStatusCode.NotFound, "phone number already exists!");

            var studentDto = new Student
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentStatus = student.StudentStatus,
                Phone = student.Phone,
                RegisteredAt = student.RegisteredAt,
                ApplicationUserId = student.ApplicationUserId
            };

            await _dbContext.Students.AddAsync(studentDto);
            await _dbContext.SaveChangesAsync();

            student.StudentId = studentDto.StudentId;

            return new Response<StudentDto>(System.Net.HttpStatusCode.OK, "Success",student);
        }
    }
}
