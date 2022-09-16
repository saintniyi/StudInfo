using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudInfoDataAccess.IRepository;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class GpaController : Controller
    {
        private readonly IUnitOfWork _uow;


        public GpaController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult GetAllDistinctGPA()
        {
            var gpaList = _uow.Gpa.GetAll()
                .OrderBy(x => x.StudNos)
                .ThenBy(x => x.Level)
                .ThenBy(x => x.Semester)
                .ToList();


            return Json(new { data = gpaList });
        }



        [HttpGet]
        public IActionResult GpaDetailsFromQryStr()
        {
            //return View("GPADetails");

            return View();
        }


        public IActionResult Process()
        {
            var scoreList = _uow.Score.GetAll(includeOthers: "Course, Grade");

            //for (int )

            //for GPA, do this for a specific studNos, in a specific Levels and specific Semesters
            //from scoreList, get unit of course from Course node store in unitAccumulator
            //from scoreList, use the score to get gpa from Grade node
            //multiply the each unit and gpa and store the result in totalWeightAccumulator
            //divide the totalWeightAccumulator by the unitAccumulator
            //the result of the division is the GPA

            //for CGPA, do this for a specific studNos for all levels and all semesters
            //you get distinct of student and repeat the steps above for a specific studNos,
            //for all levels and semesters



            ////

            return View();
        }


    }
}
