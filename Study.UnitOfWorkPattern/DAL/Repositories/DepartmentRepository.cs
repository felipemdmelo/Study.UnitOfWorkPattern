using Study.UnitOfWorkPattern.DAL.Context;
using Study.UnitOfWorkPattern.DAL.Repositories.Base;
using Study.UnitOfWorkPattern.DL.Entities;

namespace Study.UnitOfWorkPattern.DAL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(SchoolContext context) : base(context)
        {
        }
    }
}
