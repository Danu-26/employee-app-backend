using Microsoft.Data.SqlClient;
using EmployeeApp.Models;

namespace EmployeeApp.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET ALL (with department name JOIN)
        public List<Employee> GetAll()
        {
            var list = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT e.*, d.DepartmentName 
                FROM Employees e
                INNER JOIN Departments d 
                ON e.DepartmentCode = d.DepartmentCode";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Employee
                    {
                        EmployeeId = (int)reader["EmployeeId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Salary = (decimal)reader["Salary"],
                        DepartmentCode = reader["DepartmentCode"].ToString()
                    });
                }
            }

            return list;
        }

        // Check if email is unique
        public bool IsEmailUnique(string email, int excludeId = 0)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Employees WHERE Email = @email AND EmployeeId <> @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", excludeId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }

        // ADD
        public void Add(Employee emp)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Employees
                (FirstName, LastName, Email, DateOfBirth, Salary, DepartmentCode)
                VALUES (@fn, @ln, @email, @dob, @salary, @deptCode)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@fn", emp.FirstName);
                cmd.Parameters.AddWithValue("@ln", emp.LastName);
                cmd.Parameters.AddWithValue("@email", emp.Email);
                cmd.Parameters.AddWithValue("@dob", emp.DateOfBirth);
                cmd.Parameters.AddWithValue("@salary", emp.Salary);
                cmd.Parameters.AddWithValue("@deptCode", emp.DepartmentCode);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE
        public void Update(int id, Employee emp)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Employees SET
                FirstName=@fn,
                LastName=@ln,
                Email=@email,
                DateOfBirth=@dob,
                Salary=@salary,
                DepartmentCode=@deptCode
                WHERE EmployeeId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fn", emp.FirstName);
                cmd.Parameters.AddWithValue("@ln", emp.LastName);
                cmd.Parameters.AddWithValue("@email", emp.Email);
                cmd.Parameters.AddWithValue("@dob", emp.DateOfBirth);
                cmd.Parameters.AddWithValue("@salary", emp.Salary);
                cmd.Parameters.AddWithValue("@deptCode", emp.DepartmentCode);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Employees WHERE EmployeeId=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}