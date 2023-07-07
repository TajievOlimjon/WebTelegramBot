using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTelegramBot.Entities;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentDto model)
        {
            return Ok(await _studentService.AddStudentAsync(model));
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _studentService.GetAllStudentsAsync());
        }
    }
}
