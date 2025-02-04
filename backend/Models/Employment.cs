namespace backend.Models
{
    public class Employment
    {
        public int EmploymentId { get; set; }
        public string EmploymentName { get; set; }
        public int DepartmentId { get; set; } // clé étranère
        public Department Department { get; set; }
    }
}
