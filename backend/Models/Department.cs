namespace backend.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        // Relation One-to-Many (Un département a plusieurs employés)
        public ICollection<Employment> Employments { get; set; } = new List<Employment>();
    }
}
