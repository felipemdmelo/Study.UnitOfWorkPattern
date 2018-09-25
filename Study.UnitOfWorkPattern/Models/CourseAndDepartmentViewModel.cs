using System.ComponentModel.DataAnnotations;

namespace Study.UnitOfWorkPattern.Models
{
    public class CourseAndDepartmentViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string DepartmentName { get; set; }
    }
}
