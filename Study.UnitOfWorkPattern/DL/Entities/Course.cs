namespace Study.UnitOfWorkPattern.DL.Entities
{
    public class Course
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
