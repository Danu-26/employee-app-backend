namespace EmployeeApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }

        public string DepartmentCode { get; set; }

        // Auto calculated
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                if (DateOfBirth > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}