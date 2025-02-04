using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employment>>> GetEmployments()
        {
            return await _context.Employments.Include(e => e.Department).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employment>> GetEmployments(int id)
        {
            var employee = await _context.Employments.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmploymentId == id);
            
            if (employee == null)
            {
                return NotFound("Employé non trouvé");
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employment>> CreateEmployee(Employment employee)
        {
            var department = await _context.Departments.FindAsync(employee.DepartmentId);
            if (department == null)
            {
                return BadRequest("le département spécifique n'existe pas");
            }

            _context.Employments.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployments), new { id = employee.EmploymentId }, employee);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employment employee)
        {
            if (id != employee.EmploymentId)
            {
                return BadRequest("l'Id de l'employé ne correspond pas.");
            }

            var existingEmployee = await _context.Employments.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound("Employé non trouvé");
            }
            // Vérifier si le département existe
            var department = await _context.Departments.FindAsync(employee.DepartmentId);
            if (department == null)
            {
                return BadRequest("Le département spécifié n'existe pas.");
            }

            // Mise à jour des valeurs
            existingEmployee.EmploymentName = employee.EmploymentName;
            existingEmployee.DepartmentId = employee.DepartmentId;

            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employments.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employé non trouvé.");
            }

            _context.Employments.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
