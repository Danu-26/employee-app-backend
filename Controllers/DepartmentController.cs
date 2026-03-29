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

        // ✅ GET: api/Department
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
                return StatusCode(500, new { message = "Error fetching departments" });
            }
        }

        // ✅ POST: api/Department
        [HttpPost]
        public IActionResult Create([FromBody] Department dept)
        {
            try
            {
                if (dept == null || string.IsNullOrEmpty(dept.DepartmentCode) || string.IsNullOrEmpty(dept.DepartmentName))
                {
                    return BadRequest(new { message = "All fields are required." });
                }

                if (_repo.CodeExists(dept.DepartmentCode))
                {
                    return BadRequest(new { message = "Department Code must be unique." });
                }

                _repo.Add(dept);
                return Ok(new { message = "Department added successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error adding department" });
            }
        }

        // ✅ PUT: api/Department/{code}
        [HttpPut("{code}")]
        public IActionResult Update(string code, [FromBody] Department dept)
        {
            try
            {
                if (!_repo.CodeExists(code))
                {
                    return NotFound(new { message = "Department not found." });
                }

                _repo.Update(code, dept);
                return Ok(new { message = "Department updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error updating department" });
            }
        }

        // ✅ DELETE: api/Department/{code}
        [HttpDelete("{code}")]
        public IActionResult Delete(string code)
        {
            try
            {
                if (!_repo.CodeExists(code))
                {
                    return NotFound(new { message = "Department not found." });
                }

                _repo.Delete(code);
                return Ok(new { message = "Department deleted successfully" });
            }
            catch (Exception)
            {
                // ✅ Handles FK constraint error
                return BadRequest(new
                {
                    message = "Cannot delete department. It is assigned to employees."
                });
            }
        }
    }
}