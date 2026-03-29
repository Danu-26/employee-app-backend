using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _repo;

        public EmployeeController(EmployeeRepository repo)
        {
            _repo = repo;
        }

        // ✅ GET: api/Employee
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var data = _repo.GetAll();
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error fetching employees" });
            }
        }

        // ✅ POST: api/Employee
        [HttpPost]
        public IActionResult Create([FromBody] EmployeeDto dto)
        {
            try
            {
                // Validation
                if (dto == null ||
                    string.IsNullOrEmpty(dto.FirstName) ||
                    string.IsNullOrEmpty(dto.Email) ||
                    string.IsNullOrEmpty(dto.DepartmentCode))
                {
                    return BadRequest(new { message = "Required fields are missing." });
                }

                // Email unique check
                if (!_repo.IsEmailUnique(dto.Email))
                {
                    return BadRequest(new { message = "Email already exists." });
                }

                var emp = new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Salary = dto.Salary,
                    DepartmentCode = dto.DepartmentCode
                };

                _repo.Add(emp);
                return Ok(new { message = "Employee added successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error adding employee" });
            }
        }

        // ✅ PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EmployeeDto dto)
        {
            try
            {
                // Validation
                if (dto == null ||
                    string.IsNullOrEmpty(dto.FirstName) ||
                    string.IsNullOrEmpty(dto.Email) ||
                    string.IsNullOrEmpty(dto.DepartmentCode))
                {
                    return BadRequest(new { message = "Required fields are missing." });
                }

                // Email unique check (exclude current)
                if (!_repo.IsEmailUnique(dto.Email, id))
                {
                    return BadRequest(new { message = "Email already exists." });
                }

                var emp = new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Salary = dto.Salary,
                    DepartmentCode = dto.DepartmentCode
                };

                _repo.Update(id, emp);
                return Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error updating employee" });
            }
        }

        // ✅ DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repo.Delete(id);
                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error deleting employee" });
            }
        }
    }
}