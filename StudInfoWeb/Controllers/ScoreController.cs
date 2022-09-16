using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using StudInfoModel.ViewModels;
using System.Collections.Generic;
using System.Reflection;

namespace StudInfoWeb.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ScoreController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ScoreController(IUnitOfWork uow)
        {
            _uow = uow;
        }



        #region ScoreServerMethods


        public IActionResult IndexServer()
        {
            return View();
        }


        public IActionResult AddScoreServer()
        {
            ScoreVM sVM = new ScoreVM();
            sVM.Scores = _uow.Score.GetAll(x => x.StudNos == ""
            && x.Level == 0 && x.Semester == 0).ToList();

            return View("AddScoreServer", sVM);
        }



        public IActionResult ScoreDetailsServerQryStr(string studNos, string level, string semester, string grpCode)
        {
            ScoreVM sVM = new ScoreVM();

            sVM.Scores = _uow.Score.GetAll(x => x.StudNos == studNos
            && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)
            && x.GroupCode == grpCode).ToList();

            if (sVM.Scores.Count > 0)
            {
                return View("AddScoreServer", sVM);
            }
            else
            {
                EnrollmentVM eVM = new EnrollmentVM();
                eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == studNos
                && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)
                && x.GroupCode == grpCode).ToList();

                if (eVM.Enrollments.Any())
                {
                    //projects each element in the Enrollments list to Scores list
                    sVM.Scores = eVM.Enrollments.Select(x => new Score()
                    {
                        GroupCode = x.GroupCode,
                        CourseCode = x.CourseCode,
                        StudNos = x.StudNos,
                        Mark = null,
                        Level = x.Level,
                        Semester = x.Semester,
                        Course = x.Course
                    }).ToList();
                }

                return View("AddScoreServer", sVM);
            }
        }



        public IActionResult SearchScoreServer(string txtStudNos, string ddlLevel, string ddlSemester)
        {
            ScoreVM sVM = new ScoreVM();

            if (!string.IsNullOrEmpty(txtStudNos) && !string.IsNullOrEmpty(ddlLevel) && !string.IsNullOrEmpty(ddlSemester))
            {
                var stud = _uow.Student.GetOne(x => x.StudNos == txtStudNos);

                if (stud == null)
                {
                    TempData["error"] = "Student does not exist";
                    return RedirectToAction("AddScoreServer");
                }


                ViewBag.StudNos = txtStudNos;
                ViewBag.Level = ddlLevel;
                ViewBag.Semester = ddlSemester;

                sVM.Scores = _uow.Score.GetAll(x => x.StudNos == txtStudNos
                && x.Level == Convert.ToInt32(ddlLevel) && x.Semester == Convert.ToInt32(ddlSemester))
                    .OrderBy(x => x.StudNos)
                    .ThenBy(x => x.Level)
                    .ThenBy(x => x.Semester)
                    .ToList();

                if (sVM.Scores.Any())
                {
                    return View("AddScoreServer", sVM);
                }
                else
                {
                    EnrollmentVM eVM = new EnrollmentVM();
                    eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == txtStudNos
                    && x.Level == Convert.ToInt32(ddlLevel) && x.Semester == Convert.ToInt32(ddlSemester))
                        .OrderBy(x => x.StudNos)
                        .ThenBy(x => x.Level)
                        .ThenBy(x => x.Semester)
                        .ToList();

                    if (eVM.Enrollments.Count <= 0)
                    {
                        TempData["error"] = "Student not enrolled for selecetd semester and level";
                        return RedirectToAction("AddScoreServer");
                    }

                    //projects each element in the Enrollments list to Scores list
                    sVM.Scores = eVM.Enrollments.Select(x => new Score()
                    {
                        GroupCode = x.GroupCode,
                        CourseCode = x.CourseCode,
                        StudNos = x.StudNos,
                        Mark = null,
                        Level = x.Level,
                        Semester = x.Semester,
                        Processed = null
                    }).ToList();

                    return View("AddScoreServer", sVM);
                }
            }

            return View("AddScoreServer", sVM);

        }



        //This method saves score using server side approach
        //All the parameters are values from the URL which are automatically obtained  
        //with the same parameter name in the URL.
        public IActionResult SaveScoreServer(ScoreVM scoreVM, string txtStudNos, string ddlLevel, string ddlSemester)
        {
            int lev = Convert.ToInt32(ddlLevel);
            int sem = Convert.ToInt32(ddlSemester);

            var stud = _uow.Student.GetOne(x => x.StudNos == txtStudNos);

            if (stud == null)
            {
                TempData["error"] = "Student cannot be found";
                return View("AddScoreServer");
            }

            //get all grades so you can search weight and gradeLetter
            List<Grade> grades = new List<Grade>();
            grades = _uow.Grade.GetAll().OrderByDescending(x => x.Weight).ToList();

            List<Course> courses = new List<Course>();
            courses = _uow.Course.GetAll().ToList();

            float? holdMark = 0;
            string holdCourse = "";

            //Loop scoreVM.Scores and assign value for Weight, gradeLetter & Course 
            //based on the search on the grades and courses list 
            for (int k = 0; k < scoreVM.Scores.Count(); k++)
            {
                holdMark = scoreVM.Scores[k].Mark;
                holdCourse = scoreVM.Scores[k].CourseCode;

                var grade = grades.Find(x => x.MinScoreRange <= holdMark && x.MaxScoreRange >= holdMark);
                scoreVM.Scores[k].Grade = grade;   //override existing Grade properties for consistency sake
                scoreVM.Scores[k].GradeLetter = grade.GradeLetter;
                scoreVM.Scores[k].Weight = grade.Weight;

                var course = courses.Find(c => c.CourseCode == holdCourse);
                scoreVM.Scores[k].Course = course;
            }

            //get list of scores with same studnos, level & semester from the Db
            var scoresFromDb = _uow.Score.GetAll(x => x.StudNos == txtStudNos
            && x.Level == lev && x.Semester == sem).ToList();

            //if scores found, delete all such existing scores and prepare to save the new list
            if (scoresFromDb.Any())
            {
                _uow.Score.RemoveRange(scoresFromDb);
            }

            //remove existing Gpa and Cgpa
            RemoveExistingGPA_CGPA(txtStudNos, lev, sem);

            //compute gpa, add to _uow, & return the gpa as parameter for Cgpa calculation
            var gpa = ComputeGPA(scoreVM.Scores);

            //Compute Cgpa based on the Gpa passed to it
            var cgpa = ComputeCGPA(gpa, txtStudNos, lev, sem);

            //add sVM.Scores to _uow
            _uow.Score.AddRange(scoreVM.Scores);

            //add computed gpa to _uow
            _uow.Gpa.Add(gpa);

            //add computed cgpa to _uow 
            _uow.Cgpa.Add(cgpa);

            //now save scores, Gpa and Cgpa to Db
            _uow.Save();

            //update GPA and CGPA in Student table
            if (stud != null)
            {
                stud.GPA = gpa.SemesterGPA;
                stud.CGPA = cgpa.CumulativeGPA;

                _uow.Student.Update(stud);
                _uow.Save();
            }

            TempData["success"] = scoreVM.Scores.Count() + " records successfully saved.";

            return RedirectToAction("IndexServer");

        }


        #endregion ScoreServerMethods






        #region ScoreClientMethods


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddScoreClient()
        {
            ScoreVM sVM = new ScoreVM();
            sVM.Scores = _uow.Score.GetAll(x => x.StudNos == ""
            && x.Level == 0 && x.Semester == 0).ToList();

            return View("AddScoreClient", sVM);
        }


        //method invoked from the index page that load data through the client
        public IActionResult ScoreDetailsFromQryStr(string studNos, string level, string semester, string grpCode)
        {
            ScoreVM sVM = new ScoreVM();

            //it uses the parameter to search or get data from score table
            sVM.Scores = _uow.Score.GetAll(x => x.StudNos == studNos
            && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)
            && x.GroupCode == grpCode).ToList();

            //if data found, display data found from score table on score view
            if (sVM.Scores.Count > 0)
            {
                return View("AddScoreClient", sVM);
            }
            else
            {
                //if no data found, get data from Enrollment and displays on score view
                EnrollmentVM eVM = new EnrollmentVM();
                eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == studNos
                && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)
                && x.GroupCode == grpCode).ToList();

                if (eVM.Enrollments.Any())
                {
                    //projects each element in the Enrollments list to Scores list
                    sVM.Scores = eVM.Enrollments.Select(x => new Score()
                    {
                        GroupCode = x.GroupCode,
                        CourseCode = x.CourseCode,
                        StudNos = x.StudNos,
                        Mark = null,
                        Level = x.Level,
                        Semester = x.Semester
                    }).ToList();
                }

                return View("AddScoreClient", sVM);
            }
        }



        //JQuery/Ajax call from client for finding records (invoked by Find method on the client)
        [HttpGet]
        public IActionResult SearchScoreClient(string stdnos, string level, string semester)
        {
            ScoreVM sVM = new ScoreVM();

            if (!string.IsNullOrEmpty(stdnos) && !string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(semester))
            {
                //since this is invoked from the Score view, first check for existence on the score entity
                sVM.Scores = _uow.Score.GetAll(x => x.StudNos == stdnos
                && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)).ToList();

                //if found, return Json data to the caller
                if (sVM.Scores.Any())
                {
                    return Json(sVM.Scores);
                }
                else
                {
                    //if not found in Score entity, this must be first time to add to score.
                    //Therefore, get data from Enrollent entity 
                    EnrollmentVM eVM = new EnrollmentVM();
                    eVM.Enrollments = _uow.Enrollment.GetAll(x => x.StudNos == stdnos
                    && x.Level == Convert.ToInt32(level) && x.Semester == Convert.ToInt32(semester)).ToList();

                    //projects each element from the Enrollments list to Scores list
                    sVM.Scores = eVM.Enrollments.Select(x => new Score()
                    {
                        GroupCode = x.GroupCode,
                        CourseCode = x.CourseCode,
                        StudNos = x.StudNos,
                        Mark = null,
                        Level = x.Level,
                        Semester = x.Semester
                    }).ToList();

                    return Json(sVM.Scores);
                }
            }

            return Json(sVM.Scores);
        }




        //The parameter (scoreVM) is obtained when the Find button is click 
        //This makes an ajax call to GetScoreList method.
        //The data from GetScoreList may have come from Score or Enrollmemt table
        [HttpPost]
        public IActionResult SaveScores([FromBody] ScoreVM scoreVM)
        {
            string msg = "";

            List<Score> scoreListFromClient = scoreVM.Scores;

            //if studnos cannot be found, abort the save operation
            var studnos = scoreListFromClient[0].StudNos;
            var stud = _uow.Student.GetOne(x => x.StudNos == studnos);

            msg = "Scores will not be  allowed for this student";
            if (stud == null) return Json(new { success = true, msg, nos = 1 });

            List<Course> courses = new List<Course>();
            courses = _uow.Course.GetAll().ToList();

            List<Grade> grades = new List<Grade>();
            grades = _uow.Grade.GetAll().ToList();

            float? holdMark = 0;
            string holdCCode = "";

            //looping is done to get gradeLetter, Weight & CourseUnit from grades & courses list.
            //All the 3 properties are null in the incoming list and therefore could not be 
            //retrieved through their relation to scores entity
            for (int k = 0; k < scoreListFromClient.Count(); k++)
            {
                holdMark = scoreListFromClient[k].Mark;
                holdCCode = scoreListFromClient[k].CourseCode;

                var grade = grades.Find(x => x.MinScoreRange <= holdMark && x.MaxScoreRange >= holdMark);
                scoreListFromClient[k].GradeLetter = grade.GradeLetter;
                scoreListFromClient[k].Weight = grade.Weight;

                var course = courses.FirstOrDefault(x => x.CourseCode == holdCCode);
                scoreListFromClient[k].Course = course;
            }

            int? level = scoreListFromClient[0].Level;
            int? sem = scoreListFromClient[0].Semester;

            //if data for studnos, level & semester is found in Score entity,
            //it means that it comes from Score table else if not found, it comes from Enrollemt table
            //if data is from Score, delete first before inserting new one.
            //However, if data is from Enrollment, no need for delete. Insert into score table
            var scoreFromDb = _uow.Score.GetAll(x => x.StudNos == studnos && x.Level == level
            && x.Semester == sem);

            if (scoreFromDb.Any())
            {
                //delete previous exact scores from db
                _uow.Score.RemoveRange(scoreFromDb);
                //_uow.Save();
            }

            //remove existing Gpa and Cgpa
            RemoveExistingGPA_CGPA(studnos, level, sem);

            //calculate and save new gpa for current student, level and semester to Db
            var gpa = ComputeGPA(scoreListFromClient);

            //Compute cgpa for current studnos
            var cgpa = ComputeCGPA(gpa, studnos, level, sem);

            //add scores to _uow
            _uow.Score.AddRange(scoreListFromClient);

            //add computed gpa to _uow
            _uow.Gpa.Add(gpa);

            //add computed cgpa to _uow 
            _uow.Cgpa.Add(cgpa);

            //now save scores and all other modified entities to the db
            _uow.Save();

            //update GPA and CGPA in Student table
            if (stud != null)
            {
                stud.GPA = gpa.SemesterGPA;
                stud.CGPA = cgpa.CumulativeGPA;

                _uow.Student.Update(stud);
                _uow.Save();
            }

            //after Scores, Gpa & Cgpa successfully saved, show message to notify user of success
            msg = scoreListFromClient.Count + " records successfully saved";
            return Json(new { success = true, msg, nos = 2 });
        }


        #endregion ScoreClientMethods




        #region General

        public Tuple<string, float> GetGradeLetterForScore(float? score)
        {
            var result = Tuple.Create<string, float>;

            string gradeLetter = "";
            float gpa = 0;

            float start;
            float end;

            var gradeList = _uow.Grade.GetAll().ToList();

            if (gradeList.Count > 0)
            {
                for (int i = 0; i < gradeList.Count(); i++)
                {
                    start = gradeList[i].MinScoreRange;
                    end = gradeList[i].MaxScoreRange;

                    if (score >= start && score <= end)
                    {
                        gradeLetter = gradeList[i].GradeLetter;
                        gpa = gradeList[i].Weight;

                        return result(gradeLetter, gpa);
                    }
                }
            }

            return result(gradeLetter, gpa);
        }





        public Gpa ComputeGPA(List<Score> scores)
        {
            int unitAccum = 0;
            float? unitWeightAccum = 0;
            int tempUnits = 0;
            float? tempUnitsWeight = 0;
            float? temp;
            float? gpaVal = 0;

            for (int k = 0; k < scores.Count(); k++)
            {
                //get CourseUnit from Course relation and get Weight from Grade relation
                tempUnits = scores[k].Course.CourseUnit;
                tempUnitsWeight = scores[k].Weight;
                temp = tempUnits * tempUnitsWeight;

                unitAccum += tempUnits;   //accumulate total sum of units for retrieved scores in unitAccum
                unitWeightAccum += temp;  //accumulate total sum of unitweight for retrieved scores in unitWeightAccum
            }

            //calculate Gpa value here
            gpaVal = unitWeightAccum / unitAccum;

            Gpa gpa = new Gpa()
            {
                StudNos = scores[0].StudNos,
                Level = scores[0].Level,
                Semester = scores[0].Semester,
                TotalSemesterUnits = unitAccum,
                TotalSemesterUnitWeight = unitWeightAccum,
                SemesterGPA = gpaVal
            };

            return gpa;
        }



        public Cgpa ComputeCGPA(Gpa gpa, string stdnos, int? level, int? semester)
        {
            int? cumUnitSum = 0;            //cumulative unit sum up to latest Levels and semesters
            float? cumUnitWeightSum = 0;    //cumulative unitweight sum up to latest Levels and semesters
            float? cgpaVal = 0;

            //get list of Gpa for this stdnos in all levels and semester so far
            var gpaList = _uow.Gpa.GetAll(x => x.StudNos == stdnos).ToList();

            //from the gpalist, remove old Gpa for this stdnos, in this level and semester.
            //Only one record is expected to be removed with the condition
            gpaList.RemoveAll(x => x.StudNos == stdnos && x.Level == level && x.Semester == semester);

            gpaList.Add(gpa);   //add the newly current computed gpa in the parameter (coming from CalculateGPA method) to gpaList

            //calculate the cumulative unitsum & unitsum-weight in the loop
            for (int k = 0; k < gpaList.Count; k++)
            {
                cumUnitSum += gpaList[k].TotalSemesterUnits;
                cumUnitWeightSum += gpaList[k].TotalSemesterUnitWeight;
            }

            //calculate Cgpa value here
            cgpaVal = cumUnitWeightSum / cumUnitSum;

            //create a Cgpa object and populate with values from variables above
            Cgpa cgpa = new()
            {
                StudNos = stdnos,
                TotalUnits = cumUnitSum,
                TotalUnitWeight = cumUnitWeightSum,
                CumulativeGPA = cgpaVal
            };

            return cgpa;
        }



        public void RemoveExistingGPA_CGPA(string stdnos, int? level, int? semester)
        {
            //check if student, level, semester exist in Gpa, then delete first 
            var gpaFromdb = _uow.Gpa.GetAll(x => x.StudNos == stdnos &&
            x.Level == level && x.Semester == semester).ToList();
            if (gpaFromdb.Any())
            {
                //delete previous exact gpa from db
                _uow.Gpa.RemoveRange(gpaFromdb);
            }

            //check if student exist in Cgpa, then delete first 
            var cgpaFromDb = _uow.Cgpa.GetAll(x => x.StudNos == stdnos).ToList();
            if (cgpaFromDb.Any())
            {
                _uow.Cgpa.RemoveRange(cgpaFromDb);
            }
        }

        #endregion General



    }
}
