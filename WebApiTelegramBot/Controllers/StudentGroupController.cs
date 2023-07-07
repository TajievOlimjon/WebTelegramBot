using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTelegramBot.Entities;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentGroupController : ControllerBase
    {
        private readonly StudentGroupService _studentGroupService;

        public StudentGroupController(StudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentGroupDto studentGroupDto)
        {
            return Ok(await _studentGroupService.AddStudentGroupAsync(studentGroupDto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateStudentGroupDto studentGroupDto)
        {
            return Ok(await _studentGroupService.UpdateStudentGroupAsync(studentGroupDto));
        }
    }
}
