using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services.EntitiesServices
{
    public class StudentGroupService
    {
        private readonly ApplicationDbContext _context;

        public StudentGroupService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Response<AddStudentGroupDto>> AddStudentGroupAsync(AddStudentGroupDto studentGroup)
        {
            if (studentGroup.GroupId == 0 || studentGroup.StudentId == 0)
                return new Response<AddStudentGroupDto>(HttpStatusCode.NotFound, "Student or group id is not valid");
            var existingGroup = await _context.Groups.FindAsync(studentGroup.GroupId);
            if (existingGroup == null)
                return new Response<AddStudentGroupDto>(HttpStatusCode.NotFound, "Group not found");
            var existingStudent = await _context.Students.FindAsync(studentGroup.StudentId);
            if (existingStudent == null)
                return new Response<AddStudentGroupDto>(HttpStatusCode.NotFound, "Student not found");
            var existingStudentGroup = await _context.StudentGroups.FindAsync(studentGroup.GroupId, studentGroup.StudentId);
            if (existingStudentGroup != null)
            {
                return new Response<AddStudentGroupDto>(HttpStatusCode.BadRequest, "StudentGroup is already exist");
            }
            if (existingStudent.StudentGroups.Where(x => x.StudentGroupStatus == StudentGroupStatus.Active).Count() > 0)
            {
                return new Response<AddStudentGroupDto>(HttpStatusCode.BadRequest, "Student is already has an active group");
            }
            var newStudentGroup = new StudentGroup
            {
                GroupId=studentGroup.GroupId,
                StudentId=studentGroup.StudentId,
                StudentGroupStatus=studentGroup.StudentGroupStatus,
                StartedAt=DateTimeOffset.UtcNow,
                FinishedAt=DateTimeOffset.UtcNow
            };
            await _context.StudentGroups.AddAsync(newStudentGroup);
            var result = await _context.SaveChangesAsync();

            if (result == 0) return new Response<AddStudentGroupDto>(HttpStatusCode.InternalServerError, "Student not add to group !");


            return new Response<AddStudentGroupDto>(HttpStatusCode.OK, "Student successfully added to group !!!");
        }

        public async Task<Response<UpdateStudentGroupDto>> UpdateStudentGroupAsync(UpdateStudentGroupDto model)
        {
            var existingStudentGroup = await _context.StudentGroups.FindAsync(model.GroupId, model.StudentId);
            if (existingStudentGroup == null)
            {
                if (model.GroupId == 0 && model.NewGroupId != 0)
                {
                    var existingGroup = await _context.Groups.FindAsync(model.NewGroupId);
                    if (existingGroup == null) return new Response<UpdateStudentGroupDto>(HttpStatusCode.NotFound, "Group not found");

                    var newStudentGroup = new StudentGroup()
                    {
                        StudentId = model.StudentId,
                        GroupId = model.NewGroupId,
                        StudentGroupStatus = StudentGroupStatus.Active,
                        StartedAt = DateTimeOffset.UtcNow,
                        FinishedAt = DateTimeOffset.UtcNow
                    };
                    _context.StudentGroups.Add(newStudentGroup);
                    await _context.SaveChangesAsync();
                    return new Response<UpdateStudentGroupDto>(HttpStatusCode.OK,"Student added to new group");
                }
                return new Response<UpdateStudentGroupDto>(HttpStatusCode.NotFound, "Student group not found");
            }
            if (model.StudentGroupStatus == StudentGroupStatus.Left)
            {
                existingStudentGroup.StudentGroupStatus = model.StudentGroupStatus;
                await _context.SaveChangesAsync();
                return new Response<UpdateStudentGroupDto>(HttpStatusCode.OK,"Student group updated successfully");
            }
            if (model.StudentGroupStatus == StudentGroupStatus.Finished)
            {
                var existingGroup = await _context.Groups.FindAsync(model.GroupId);
                if (existingGroup == null) return new Response<UpdateStudentGroupDto>(HttpStatusCode.NotFound, "Group not found");

                existingStudentGroup.StudentGroupStatus = StudentGroupStatus.Finished;
                existingStudentGroup.FinishedAt = DateTimeOffset.UtcNow;

                var studentInNewGroup = await _context.StudentGroups.FindAsync(model.NewGroupId, model.StudentId);
                if (studentInNewGroup != null) return new Response<UpdateStudentGroupDto>(HttpStatusCode.Found, "Student is already in new group");
                
                //create new group
                var newStudentGroup = new StudentGroup
                {
                    StudentId = model.StudentId,
                    GroupId = model.NewGroupId,
                    StudentGroupStatus = StudentGroupStatus.Active,
                    StartedAt = DateTimeOffset.UtcNow
                };
                await _context.StudentGroups.AddAsync(newStudentGroup);
                await _context.SaveChangesAsync();
                return new Response<UpdateStudentGroupDto>(HttpStatusCode.OK, "Student group updated successfully");
            }
            existingStudentGroup.StudentGroupStatus = model.StudentGroupStatus;
            existingStudentGroup.StartedAt = existingStudentGroup.StartedAt;
            existingStudentGroup.StudentId = existingStudentGroup.StudentId;
            existingStudentGroup.FinishedAt = existingStudentGroup.FinishedAt;

            await _context.SaveChangesAsync();
            return new Response<UpdateStudentGroupDto>(HttpStatusCode.OK, "Student group updated successfully");
        }
    }
}
