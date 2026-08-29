using CollegeAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CollegeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentRepository.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByID(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }
}
