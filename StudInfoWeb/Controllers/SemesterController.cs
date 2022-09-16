using Microsoft.AspNetCore.Mvc;
using StudInfoDataAccess.IRepository;

namespace StudInfoWeb.Controllers
{
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _uow;


        public SemesterController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public IActionResult End()
        {
            return View();
        }


        public IActionResult Close(string ddlLevel, string ddlSemester)
        {
            int lev = Convert.ToInt32(ddlLevel);
            int sem = Convert.ToInt32(ddlSemester);

            var listEnrol = _uow.Enrollment.GetAll(x => x.Level == lev && x.Semester == sem).ToList();
            var listScores = _uow.Score.GetAll(x => x.Level == lev && x.Semester == sem).ToList();

            for (int k = 0; k < listEnrol.Count(); k++)
            {
                listEnrol[k].Processed = "p";
            }

            for (int k = 0; k < listScores.Count(); k++)
            {
                listScores[k].Processed = "p";
            }

           _uow.Enrollment.UpdateRange(listEnrol);
            
           _uow.Score.UpdateRange(listScores);
           _uow.Save();

            TempData["success"] = "Semester successfully closed";


            return RedirectToAction("End");
        }



    }
}
