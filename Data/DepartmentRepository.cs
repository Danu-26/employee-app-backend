using Microsoft.Data.SqlClient;
using EmployeeApp.Models;

namespace EmployeeApp.Data
{
    public class DepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Get all departments
        public List<Department> GetAll()
        {
            var list = new List<Department>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT DepartmentCode, DepartmentName FROM Departments";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Department
                    {
                        DepartmentCode = reader["DepartmentCode"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    });
                }
            }
            return list;
        }

        // Check if code exists
        public bool CodeExists(string code)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Departments WHERE DepartmentCode = @code";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", code);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Add new department
        public void Add(Department dept)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Departments (DepartmentCode, DepartmentName) VALUES (@code, @name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", dept.DepartmentCode);
                cmd.Parameters.AddWithValue("@name", dept.DepartmentName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Update department
        public void Update(string code, Department dept)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Departments SET DepartmentName=@name WHERE DepartmentCode=@code";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@name", dept.DepartmentName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete department
        public void Delete(string code)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Departments WHERE DepartmentCode=@code";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", code);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}