using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTelegramBot.Entities;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(GroupDto groupDto)
        {
            return Ok(await _groupService.AddGroupAsync(groupDto));
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _groupService.GetAllGroupsAsync());
        }
    }
}
