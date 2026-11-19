using Lager_N_Tier_arkitektur.Dtos;
using Lager_N_Tier_arkitektur.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lager_N_Tier_arkitektur.Controllers
{
    // =================================================================
    //  PRESENTATION LAYER (Presentationslager)  -  CONTROLLER
    // -----------------------------------------------------------------
    //  Only job: HTTP in -> call the Business layer -> HTTP out.
    //  No DbContext, no EF Core, no business rules here.
    //  It only knows IStudentService and the DTOs.
    // =================================================================
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        // GET: api/Students
        // Flow: Controller -> StudentService -> StudentRepository -> DbContext -> SQL Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _service.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, SaveStudentDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(SaveStudentDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, created);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
