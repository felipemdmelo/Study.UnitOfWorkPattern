using Study.UnitOfWorkPattern.DAL.Repositories;
using System.Threading.Tasks;

namespace Study.UnitOfWorkPattern.DAL
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task Save();
        void Dispose();
        ICourseRepository CourseRepository();
        IDepartmentRepository DepartmentRepository();
        IStudentRepository StudentRepository();
    }
}
