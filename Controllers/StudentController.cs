using CollegeAPI.Models;
using CollegeAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CollegeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentRepository studentRepository, ILogger<StudentController> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var students = await _studentRepository.GetAllAsync();
            return Ok(students);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while getting all students.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting all students.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByID(int id)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student is null)
            {
                return NotFound();
            }

            return Ok(student);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while getting student {StudentID}.", id);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting student {StudentID}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent([FromBody] Student student)
    {
        try
        {
            var newStudentId = await _studentRepository.AddAsync(student);
            student.StudentID = newStudentId;
            return CreatedAtAction(nameof(GetByID), new { id = newStudentId }, student);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while adding a student.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding a student.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditStudent(int id, [FromBody] Student student)
    {
        try
        {
            student.StudentID = id;
            var updated = await _studentRepository.UpdateAsync(student);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while updating student {StudentID}.", id);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating student {StudentID}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id, [FromQuery] string modifiedBy)
    {
        try
        {
            var deleted = await _studentRepository.DeleteAsync(id, modifiedBy);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while deleting student {StudentID}.", id);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting student {StudentID}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}
