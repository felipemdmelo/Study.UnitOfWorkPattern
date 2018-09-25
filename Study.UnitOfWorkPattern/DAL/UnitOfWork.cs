using Study.UnitOfWorkPattern.DAL.Context;
using Study.UnitOfWorkPattern.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace Study.UnitOfWorkPattern.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SchoolContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IStudentRepository _studentRepository;

        public UnitOfWork(SchoolContext context, ICourseRepository courseRepository, 
            IDepartmentRepository departmentRepository, IStudentRepository studentRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _studentRepository = studentRepository;
        }

        public void BeginTransaction()
        {
            if (_context.Database.CurrentTransaction != null) return;

            _context.Database.BeginTransaction();
            //_disposed = false;
        }

        public void CommitTransaction()
        {
            if (_context.Database.CurrentTransaction == null) return;

            _context.SaveChanges();
            _context.Database.CurrentTransaction.Commit();
        }

        public void RollbackTransaction()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ICourseRepository CourseRepository()
        {
            return _courseRepository;
        }

        public IDepartmentRepository DepartmentRepository()
        {
            return _departmentRepository;
        }

        public IStudentRepository StudentRepository()
        {
            return _studentRepository;
        }
    }
}
