using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.UnitOfWorkPattern.DAL;
using Study.UnitOfWorkPattern.DL.Entities;

namespace Study.UnitOfWorkPattern.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.StudentRepository().Get());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var student = await _unitOfWork.StudentRepository().GetByID(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.StudentRepository().Insert(student);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var student = await _unitOfWork.StudentRepository().GetByID(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.StudentRepository().Update(student);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var student = await _unitOfWork.StudentRepository().GetByID(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            _unitOfWork.StudentRepository().Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(long id)
        {
            return _unitOfWork.StudentRepository().GetByID(id) != null;
        }
    }
}
