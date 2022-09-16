using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel.ViewModels;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CourseController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public IActionResult Index()
        {
            //Pa$$word123
            return View();
        }


        // GET: CourseController/Upsert/id
        public IActionResult Upsert(int? id)
        {
            CourseVM courseVM = new()
            {
                //initialise new course model here
                Course = new(),

                //load DeptList properties here
                DeptList = _uow.Dept.GetAll().Select(x => new SelectListItem
                {
                    Text = x.DeptName,
                    Value = x.DeptCode
                }),

                //load Levellist here
                LevelList = LoadLevels(),

                //load SemesterList here
                SemesterList = LoadSemesters()
            };


            if (id == null || id == 0)
            {
                //this is an insert mode, ccode represent parameter for course code
                //will be null or empty string
                return View(courseVM);
            }
            else
            {
                //this is an update mode, ccode must have a value
                courseVM.Course = _uow.Course.GetOne(x => x.Id == id);    //, includeOthers: "Dept");
                return View(courseVM);
            }

        }


        public List<SelectListItem> LoadLevels()
        {
            List<string> list = new List<string>();

            for (int k = 100; k < 600; k = k + 100)
            {
                list.Add(k.ToString());
            }

            var levels = list.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString()
            }).ToList();

            levels.Insert(0, new SelectListItem() { Text = "--Select--", Value = "" });

            return levels;

        }



        public List<SelectListItem> LoadSemesters()
        {
            List<string> list = new List<string>();

            for (int k = 1; k < 3; k++)
            {
                list.Add(k.ToString());
            }

            var semesters = list.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString()
            }).ToList();

            semesters.Insert(0, new SelectListItem() { Text = "--Select--", Value = "" });

            return semesters;
        }




        // POST: CourseController/Upsert/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CourseVM courseVM)
        {
            if (ModelState.IsValid)
            {
                if (courseVM.Course.Id == 0)
                {
                    //this is an insert operation
                    _uow.Course.Add(courseVM.Course);
                    TempData["success"] = "Course created successfully";
                }
                else
                {
                    //this is an update operation
                    _uow.Course.Update(courseVM.Course);
                    TempData["success"] = "Course updated successfully";
                }

                _uow.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View("courseVM");
            }

        }


        // GET:CourseController/Delete/CourseCode
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id != null && id != 0)
        //    {
        //        CourseVM courseVM = new();
        //        courseVM.Course = _uow.Course.GetOne(x => x.Id == id);
        //        courseVM.DeptList = _uow.Dept.GetAll().Select(x => new SelectListItem
        //        {
        //            Text = x.DeptName,
        //            Value = x.DeptCode
        //        });



        //        if (courseVM.Course != null)
        //            return View(courseVM);
        //        else
        //            return NotFound();
        //    }
        //    else
        //    {
        //        ViewBag.ErrorTitle = "Null or empty Course Code";
        //        ViewBag.ErrorMessage = "Null or empty Course Code in Delete method of Course controller";
        //        return View("ErrorView");
        //    }
        //}




        #region API CALLS


        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courseList = _uow.Course.GetAll();
            return Json(new { data = courseList });
        }


        // DELETE: CourseController/RemoveCourse/5
        [HttpDelete]
        public IActionResult DeleteCourse(int id)
        {

            var course = _uow.Course.GetOne(x => x.Id == id);
            if (course == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _uow.Course.Remove(course);
            _uow.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion


    }
}
