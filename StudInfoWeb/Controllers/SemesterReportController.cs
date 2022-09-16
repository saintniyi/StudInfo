using Microsoft.AspNetCore.Mvc;
using StudInfoDataAccess.IRepository;
using StudInfoModel.ViewModels;
using StudInfoModel;
using Microsoft.AspNetCore.Authorization;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class SemesterReportController : Controller
    {
        public readonly IUnitOfWork _uow;

        public SemesterReportController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            ReportVM rVM = new();
            rVM.Scores = _uow.Score.GetAll(x => x.StudNos == ""
            && x.Level == 0 && x.Semester == 0).ToList();

            return View(rVM);
        }



        public IActionResult IndexAllStud()
        {
            ReportVM rVM = new();
            rVM.Scores = _uow.Score.GetAll(x => x.Level == 0 && x.Semester == 0).ToList();

            return View(rVM);
        }




        public IActionResult Search(string txtStudNos, string ddlLevel, string ddlSemester)
        {
            ReportVM rVM = new ();

            if (!string.IsNullOrEmpty(txtStudNos) && !string.IsNullOrEmpty(ddlLevel) && !string.IsNullOrEmpty(ddlSemester))
            {
                var stud = _uow.Student.GetOne(x => x.StudNos == txtStudNos);

                if (stud == null)
                {
                    TempData["error"] = "Student does not exist";
                    return RedirectToAction("Index");
                }

                rVM.Name= stud.FirstName + " " + stud.LastName;

                ViewBag.StudNos = txtStudNos;
                ViewBag.Level = ddlLevel;
                ViewBag.Semester = ddlSemester;

                int lev = Convert.ToInt32(ddlLevel);
                int sem = Convert.ToInt32(ddlSemester);

                rVM.Scores = _uow.Score.GetAll(x => x.StudNos == txtStudNos
                && x.Level == lev && x.Semester == sem)
                    .OrderBy(x => x.StudNos)
                    .ThenBy(x => x.Level)
                    .ThenBy(x => x.Semester)
                    .ToList();

                rVM.Gpa = _uow.Gpa.GetOne(x => x.StudNos == txtStudNos && x.Level == lev
                && x.Semester == sem);

                rVM.Cgpa = _uow.Cgpa.GetOne(x => x.StudNos == txtStudNos);

                return View("Index", rVM);
            }

            return View("Index", rVM);

        }


       
        public IActionResult SearchAll(string ddlLevel, string ddlSemester)
        {
            ReportVM rVM = new();

            if (!string.IsNullOrEmpty(ddlLevel) && !string.IsNullOrEmpty(ddlSemester))
            {


                // rVM.Name = stud.FirstName + " " + stud.LastName;

                //ViewBag.StudNos = txtStudNos;

                ViewBag.Level = ddlLevel;
                ViewBag.Semester = ddlSemester;

                int lev = Convert.ToInt32(ddlLevel);
                int sem = Convert.ToInt32(ddlSemester);

                rVM.Scores = _uow.Score.GetAll(x => x.Level == lev && x.Semester == sem)
                    .OrderBy(x => x.StudNos)
                    .ThenBy(x => x.Level)
                    .ThenBy(x => x.Semester)
                    .ToList();

                var distinctGrpCodeList = _uow.Score.GetAll(x => x.Level == lev && x.Semester == sem)
                    .Select(x => x.GroupCode).Distinct().ToList();

                //List<string> lstGrpCode = distinctGrpCodeList;

                rVM.DistinctGroupCode = distinctGrpCodeList;

                //foreach (var grpCode in lstGrpCode)
                //{
                //    rVM.DistinctGroupCode.Add(grpCode);
                //}


                rVM.Gpas = _uow.Gpa.GetAll(x => x.Level == lev
                && x.Semester == sem).ToList();

                rVM.Cgpas = _uow.Cgpa.GetAll().ToList();

                return View("IndexAllStud", rVM);
            }

            return View("IndexStud", rVM);

        }







    }
}
