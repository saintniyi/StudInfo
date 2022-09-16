using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using StudInfoModel.ViewModels;

namespace StudInfoWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrollmentController : Controller
    {
        private readonly IUnitOfWork _uow;

        public EnrollmentController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public IActionResult Index()
        {
            return View();
        }



        public IActionResult OldEnrol_1()
        {
            return View("oldEnrollCourse1");
        }



        public IActionResult OldEnrol_2()
        {
            return View("oldEnrollCourse2");
        }



        public IActionResult AddEnrollmentClient()
        {
            EnrollmentVM eVM = new EnrollmentVM();
            eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == ""
            && x.Level == 0 && x.Semester == 0).ToList();

            return View("AddEnrollmentClient", eVM);
        }




        public IActionResult EnrollmentDetailsFromQryStr(string studNos, string level, string semester, string grpCode)
        {
            EnrollmentVM eVM = new EnrollmentVM();

            //eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == studNos
            //&& x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)
            //&& x.GroupCode == grpCode).ToList();

            eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == ""
            && x.Level == 0 && x.Semester == 0 && x.GroupCode == "").ToList();

            return View("AddEnrollmentClient", eVM);
        }


        public IActionResult LoadEnrollmentsFromIndexView(string studNos, string level, string semester, string grpCode)
        {
            EnrollmentVM eVM = new EnrollmentVM();

            eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == studNos
                 && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester) && x.GroupCode == grpCode).ToList();

            return View("AddEnrollmentClient", eVM);
        }




        [HttpGet]
        public IActionResult GetAllDistinctEnrollments()
        {
            var enroList = _uow.Enrollment.GetAll(x => x.Processed != "p").OrderBy(x => x.GroupCode)
                .ThenBy(x => x.StudNos)
                .ThenBy(x => x.Level)
                .ThenBy(x => x.Semester)
                .Select(x => new { x.GroupCode, x.StudNos, x.Level, x.Semester }).Distinct();

            return Json(new { data = enroList });
        }




        [HttpGet]
        public IActionResult SearchEnrollmentFromClient(string stdnos, string level, string semester)
        {
            string msg = "";

            if (!string.IsNullOrEmpty(stdnos) && !string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(semester))
            {
                //if studnos cannot be found in student table, abort the search operation
                var stud = _uow.Student.GetOne(x => x.StudNos == stdnos);

                msg = "Student does not exist. operation aborted";
                if (stud == null) return Json(new { success = true, msg, nos = 1 });

                EnrollmentVM eVM = new EnrollmentVM();
                eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == stdnos
                && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester))
                .OrderBy(x => x.GroupCode)
                .ThenBy(x => x.StudNos)
                .ThenBy(x => x.Level)
                .ThenBy(x => x.Semester)
                .ToList();

                if(eVM.Enrollments.Count > 0)
                {
                    return Json(eVM.Enrollments);
                }
                else
                {
                    msg = "";
                    return Json(new { success = true, msg, nos = 2 });
                }

            }

            msg = "Please, enter value for student nos, level and semester";
            return Json(new { success = true, msg, nos = 3 });
        }



        [HttpPost]
        public IActionResult SaveEnrollment([FromBody] EnrollmentVM enrollmentVM)
        {
            string gcode = "";

            //before saving, check these records never exist
            string msg = "";

            if (enrollmentVM.Enrollments == null) return Json(new { msg = "No enrollment to save", nos = "1" });
            if (enrollmentVM.Enrollments.Count <= 0) return Json(new { msg = "No enrollment to save", nos = "2" });

            //store the list of enrollments to be save in enrollmentList
            List<Enrollment> enroListFromClient = enrollmentVM.Enrollments;


            //check at the server side to ensure each course is selected once on the table
            var query = enroListFromClient.GroupBy(x => x.CourseCode)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            msg = "A course is selected more than once in the table. Select each course once.";
            if (query.Count > 0) return Json(new { success = true, msg, nos = 3 });

            //if studnos cannot be found in student table, abort the save operation
            string studnos = enroListFromClient[0].StudNos;
            var stud = _uow.Student.GetOne(x => x.StudNos == studnos);

            msg = "Enrollment is not allowed for this student";
            if (stud == null) return Json(new { success = true, msg, nos = 4 });


            //get GroupCode from data coming from client. If one has value, all must have value
            gcode = enroListFromClient[0].GroupCode;

            if (string.IsNullOrEmpty(gcode))
            {
                //it means this is a new enrollment, generate a new sequence
                int id = _uow.GetNextValue();
                string grpcode = "GRP-" + id.ToString();
                gcode = grpcode;

                //insert grpcode into enroListFromClient.GroupCode field for the whole list
                foreach (var enrollment in enroListFromClient)
                {
                    enrollment.GroupCode = grpcode;
                }
            }
            else
            {
                //it is an existing enrollments with the same GroupCode or (StudNos, Level & Semester)
                //in the incoming list and can also be found in the db
                string stdnos = enroListFromClient[0].StudNos;
                int lev = enroListFromClient[0].Level;
                int sem = enroListFromClient[0].Semester;

                //var enrolFromDb = _uow.Enrollment.GetAll(x => x.GroupCode == enroListFromClient[0].GroupCode).ToList();
                var enrolFromDb = _uow.Enrollment.GetAll(x => x.StudNos == stdnos && x.Level == lev && x.Semester == sem).ToList();

                //found enrollments are deleted from the database
                if (enrolFromDb != null)
                {
                    if (enrolFromDb.Any())
                    {
                        _uow.Enrollment.RemoveRange(enrolFromDb);
                        _uow.Save();
                    }
                }
            }

            //now save the incoming enrollments list in the database
            _uow.Enrollment.AddRange(enroListFromClient);
            _uow.Save();


            //after enrollments are saved update student table as well
            if (stud != null)
            {
                stud.Level = enroListFromClient[0].Level;
                stud.Semester = enroListFromClient[0].Semester;
                stud.LatestEnrollGrpCode = gcode;
                _uow.Student.Update(stud);
                _uow.Save();
            }
            
            msg = enroListFromClient.Count + " records successfully saved";
            return Json(new { success = true, msg, nos = 5 });

        }



        [HttpPost]
        public IActionResult LoadCourses()
        {
            EnrollmentVM enrollmentVM = new EnrollmentVM();

            var courseListItem = _uow.Course.GetAll()
            .Select(x => new SelectListItem
            {
                Text = x.CourseCode,
                Value = x.CourseCode
            }).ToList();

            courseListItem.Insert(0, new SelectListItem("--Select--", ""));

            enrollmentVM.CourseList = courseListItem;

            return Json(enrollmentVM.CourseList);
        }


        //url: "@Url.Action("SearchEnrollmentFromClient")",   //"/Score/SearchEnrollmentFromClient",
        //        data: { stdnos: txtStudNos, level: ddlLevel, semester: ddlSemester },
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",


        [HttpPost]
        public IActionResult LoadCoursesByLevelAndSemester(int level, int semester)
        {
            EnrollmentVM enrollmentVM = new EnrollmentVM();

            var courseListItem = _uow.Course.GetAll(x => x.Level == level && x.Semester == semester)
            .Select(x => new SelectListItem
            {
                Text = x.CourseCode,
                Value = x.CourseCode
            }).ToList();

            courseListItem.Insert(0, new SelectListItem("--Select--", ""));

            enrollmentVM.CourseList = courseListItem;

            return Json(enrollmentVM.CourseList);
        }




        [HttpPost]
        public IActionResult LoadStudNos()
        {
            EnrollmentVM enrollmentVM = new EnrollmentVM();

            var stdListItem = _uow.Student.GetAll()
            .Select(x => new SelectListItem
            {
                Text = x.StudNos,
                Value = x.StudNos
            }).ToList();

            stdListItem.Insert(0, new SelectListItem("--Select--", ""));

            enrollmentVM.StudentList = stdListItem;

            return Json(enrollmentVM.StudentList);
        }


        [HttpPost]
        public IActionResult LoadLevels()
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

            return Json(levels);

        }


        [HttpPost]
        public IActionResult LoadSemesters()
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

            return Json(semesters);
        }



    }
}
