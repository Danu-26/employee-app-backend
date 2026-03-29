using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Data;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository _repo;

        public DepartmentController(DepartmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Department dept)
        {
            if (_repo.CodeExists(dept.DepartmentCode))
            {
                return BadRequest(new { message = "Department Code must be unique." });
            }
            _repo.Add(dept);
            return Ok(new { message = "Department added" });
        }

        [HttpPut("{code}")]
        public IActionResult Update(string code, [FromBody] Department dept)
        {
            if (!_repo.CodeExists(code))
            {
                return NotFound(new { message = "Department not found." });
            }
            _repo.Update(code, dept);
            return Ok(new { message = "Department updated" });
        }

        [HttpDelete("{code}")]
        public IActionResult Delete(string code)
        {
            if (!_repo.CodeExists(code))
            {
                return NotFound(new { message = "Department not found." });
            }
            _repo.Delete(code);
            return Ok(new { message = "Department deleted" });
        }
    }
}