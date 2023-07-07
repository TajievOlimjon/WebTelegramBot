using Microsoft.EntityFrameworkCore;
using WebApiTelegramBot.Data;
using WebApiTelegramBot.Entities;

namespace WebApiTelegramBot.Services.EntitiesServices
{
    public class GroupService
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
        {
            var groups = await _dbContext.Groups.Select(group=> new GroupDto
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupStatus = group.GroupStatus,
                CourseFormat = group.CourseFormat,
                DurationType = group.DurationType,
                FinishedAt = group.FinishedAt,
                StartsAt = group.StartsAt
            }).ToListAsync();

            return groups;
        }
        public async Task<Response<GroupDto>> GetGroupAsync(int groupId)
        {
            var group = await _dbContext.Groups.FindAsync(groupId);
            if (group == null) return new Response<GroupDto>(System.Net.HttpStatusCode.NotFound, "Data not found !");

            var groupDto = new GroupDto
            {
                GroupId=group.GroupId,
                GroupName=group.GroupName,
                GroupStatus=group.GroupStatus,
                CourseFormat=group.CourseFormat,
                DurationType=group.DurationType,
                FinishedAt=group.FinishedAt,
                StartsAt=group.StartsAt
            };

            return new Response<GroupDto>(System.Net.HttpStatusCode.NotFound, "Data successfully found !",groupDto);
        }
        public async Task<Response<GroupDto>> AddGroupAsync(GroupDto group)
        {
            var model = new Group
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupStatus = group.GroupStatus,
                CourseFormat = group.CourseFormat,
                DurationType = group.DurationType,
                FinishedAt = group.FinishedAt,
                StartsAt = group.StartsAt
            };

            await _dbContext.Groups.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            group.GroupId = model.GroupId;

            return new Response<GroupDto>(System.Net.HttpStatusCode.NotFound, "Data successfully added !", group);
        }
    }
}
