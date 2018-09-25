using Study.UnitOfWorkPattern.DAL.Context;
using Study.UnitOfWorkPattern.DAL.Repositories.Base;
using Study.UnitOfWorkPattern.DL.Entities;

namespace Study.UnitOfWorkPattern.DAL.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(SchoolContext context) : base(context)
        {
        }
    }
}
