using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study.UnitOfWorkPattern.DAL;
using Study.UnitOfWorkPattern.DL.Entities;
using Study.UnitOfWorkPattern.Models;

namespace Study.UnitOfWorkPattern.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {            
            return View(await _unitOfWork.CourseRepository().Get(includeProperties: "Department"));
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository().GetOne(m => m.Id == id, includeProperties: "Department");
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            await LoadViewBags();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentId")] Course course)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.CourseRepository().Insert(course);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            await LoadViewBags(course.DepartmentId);
            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> CreateCourseAndDepartment()
        {
            await LoadViewBags();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourseAndDepartment([Bind("CourseName,DepartmentName")] CourseAndDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.BeginTransaction();

                    // Creating Department object..
                    var department = new Department
                    {
                        Name = model.DepartmentName
                    };
                    await _unitOfWork.DepartmentRepository().Insert(department);

                    //ForcingErrorForRollBackAction(); // Forcing error..

                    // Creating Course object..
                    var course = new Course
                    {
                        Name = model.CourseName,
                        DepartmentId = department.Id
                    };                    
                    await _unitOfWork.CourseRepository().Insert(course);

                    _unitOfWork.CommitTransaction();
                    //await _unitOfWork.Save(); SaveChanges() command is not necessary. It is already called into CommitTransaction()..

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("error", e.Message);
                    _unitOfWork.RollbackTransaction();
                }
            }

            return View(model);
        }

        private void ForcingErrorForRollBackAction()
        {
            throw new Exception("FORCING ERROR TO ROLLBACK ACTION");
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository().GetByID(id);
            if (course == null)
            {
                return NotFound();
            }
            await LoadViewBags(course.DepartmentId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,DepartmentId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CourseRepository().Update(course);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await LoadViewBags(course.DepartmentId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository().GetOne(m => m.Id == id, includeProperties: "Department");
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var course = await _unitOfWork.CourseRepository().GetByID(id);
            _unitOfWork.CourseRepository().Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CourseExists(long id)
        {
            return await _unitOfWork.CourseRepository().GetByID(id) != null;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }

        #region PRIVATE METHODS
        private async Task LoadViewBags(long departmentId = 0)
        {
            ViewData["DepartmentId"] = new SelectList(await _unitOfWork.DepartmentRepository().Get(orderBy: q => q.OrderBy(d => d.Name)), "Id", "Name", departmentId);
        }
        #endregion
    }
}
