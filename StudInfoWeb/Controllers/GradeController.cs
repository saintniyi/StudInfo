using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using StudInfoModel.ViewModels;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly IUnitOfWork _uow;


        public GradeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Grade> gradeList = _uow.Grade.GetAll()
                .OrderBy(x => x.Id);
            return View(gradeList);
        }



        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Grade grade = new();

            if (id == null || id == 0)
            {
                //this is an insert mode, ID which represent identity value
                //will be null or zero
                return View(grade);
            }
            else
            {
                //this is an update mode, ID must have a value
                grade = _uow.Grade.GetOne(x => x.Id == id);
                return View(grade);
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Grade grade)
        {

            if (ModelState.IsValid)
            {
                if (grade.Id == 0)
                {
                    //this is an insert operation
                    _uow.Grade.Add(grade);
                    TempData["success"] = "Grade created successfully";
                }
                else
                {
                    //this is an update operation
                    _uow.Grade.Update(grade);
                    TempData["success"] = "Grade updated successfully";
                }

                _uow.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View("grade");
            }

        }




        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                var gradeFromDb = _uow.Grade.GetOne(x => x.Id == id);

                if (gradeFromDb == null)
                {
                    return NotFound();
                }

                return View(gradeFromDb);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGrade(Grade grade)
        {
            if (grade == null)
            {
                return NotFound();
            }

            _uow.Grade.Remove(grade);
            _uow.Save();
            TempData["success"] = "Grade successfully deleted";
            return RedirectToAction("Index");
        }






    }
}
