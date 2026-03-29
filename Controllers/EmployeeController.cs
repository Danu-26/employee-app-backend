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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] EmployeeDto dto)
        {
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
            return Ok(new { message = "Employee Added" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EmployeeDto dto)
        {
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
            return Ok(new { message = "Employee Updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return Ok("Employee Deleted");
        }
    }
}