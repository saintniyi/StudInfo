using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudInfoDataAccess.IRepository;
using StudInfoModel;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        //private readonly IFacultyRepository _db;
        private readonly IUnitOfWork _uow;

        //public FacultyController(IFacultyRepository db)
        //{
        //    _db = db;
        //}

        public FacultyController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Faculty> facultyList = _uow.Faculty.GetAll();
            return View(facultyList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Faculty fac)
        {
            if (ModelState.IsValid)
            {
                _uow.Faculty.Add(fac);
                _uow.Save();
                TempData["success"] = "Faculty created successfully";
                return RedirectToAction("Index");
            }
            return View(fac);
        }


        [HttpGet]
        public IActionResult Edit(string fac_code)
        {
            if (!string.IsNullOrEmpty(fac_code))
            {
                var facFromDb = _uow.Faculty.GetOne(x => x.FacultyCode == fac_code);

                if (facFromDb == null)
                {
                    return NotFound();
                }

                return View(facFromDb);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Faculty fac)
        {
            if (ModelState.IsValid)
            {
                _uow.Faculty.Update(fac);
                _uow.Save();
                TempData["success"] = "Faculty successfully updated";
                return RedirectToAction("Index");
            }

            return View(fac);
        }


        [HttpGet]
        public IActionResult Delete(string fac_code)
        {
            if (!string.IsNullOrEmpty(fac_code))
            {
                var facFromDb = _uow.Faculty.GetOne(x => x.FacultyCode == fac_code);

                if (facFromDb == null)
                {
                    return NotFound();
                }

                return View(facFromDb);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Faculty fac)
        {
            if (fac == null)
            {
                return NotFound();
            }

            _uow.Faculty.Remove(fac);
            _uow.Save();
            TempData["success"] = "Faculty successfully deleted";
            return RedirectToAction("Index");
        }






    }
}
