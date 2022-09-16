using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using StudInfoModel.ViewModels;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class DeptController : Controller
    {
        public readonly IUnitOfWork _uow;

        public DeptController(IUnitOfWork uow)
        {
            _uow = uow;
        }



        public IActionResult Index(string searchString, string sortField,
            string currentSortField, string currentSortOrder, int? pageNo,
            string currentFilter)
        {
            IEnumerable<Dept> deptList = new List<Dept>();

            if (searchString != null)
            {
                pageNo = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentSort"] = sortField;
            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                //search data using any of the field as filters
                searchString = searchString.ToLower().Trim();

                deptList = _uow.Dept.GetAll(
                    x => x.DeptCode.ToLower().Contains(searchString)
                    || x.DeptName.ToLower().Contains(searchString)
                    || x.Faculty.FacultyName.ToLower().Contains(searchString),
                    includeOthers: "Faculty");
            }
            else
            {
                //returns all data in no filter is speified
                deptList = _uow.Dept.GetAll(includeOthers: "Faculty");
            }

            //sort data
            deptList = SortDeptData(deptList, sortField, currentSortField, currentSortOrder);


            int pageSize = 10;

            return View(PagingList<Dept>.CreateAsync(deptList.AsQueryable(), pageNo ?? 1, pageSize));
        }


        private IEnumerable<Dept> SortDeptData(IEnumerable<Dept> depts, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "DeptCode";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if (currentSortField == sortField)
                {
                    ViewBag.SortOrder = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(Dept).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                depts = depts.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                depts = depts.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            return depts;
        }



        // GET: DeptController/Upsert/ID
        public IActionResult Upsert(int? ID)
        {
            DeptVM deptVM = new()
            {
                Dept = new(),

                FacultyList = _uow.Faculty.GetAll().Select(x => new SelectListItem
                {
                    Text = x.FacultyName,
                    Value = x.FacultyCode
                })
            };

            if (ID == null || ID == 0)
            {
                //this is an insert mode, ID which represent identity value
                //will be null or zero
                return View(deptVM);
            }
            else
            {
                //this is an update mode, ID must have a value
                deptVM.Dept = _uow.Dept.GetOne(x => x.ID == ID);
                return View(deptVM);
            }

        }



        // POST: DeptController/Upsert/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(DeptVM depVM)
        {

            if (ModelState.IsValid)
            {
                if (depVM.Dept.ID == 0)
                {
                    //this is an insert operation
                    _uow.Dept.Add(depVM.Dept);
                    TempData["success"] = "Dept created successfully";
                }
                else
                {
                    //this is an update operation
                    _uow.Dept.Update(depVM.Dept);
                    TempData["success"] = "Dept updated successfully";
                }

                _uow.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View("depVM");
            }

        }



        //THE SERVER SIDE BUTTON FOR THIS ACTION METHOD HAS BEEN COMMENTED AND
        //CLIENT SIDE BUTTON USED IN ITS STEAD

        //// GET: DeptController/Delete/5
        //[HttpGet]
        //public IActionResult Delete(int ID)
        //{
        //    if (ID != 0)
        //    {
        //        DeptVM depVM = new();
        //        depVM.Dept = _uow.Dept.GetOne(x => x.ID == ID);
        //        depVM.FacultyList = _uow.Faculty.GetAll().Select(x => new SelectListItem
        //        {
        //            Text = x.FacultyName,
        //            Value = x.FacultyCode
        //        });

        //        if (depVM.Dept != null)
        //            return View(depVM);
        //        else
        //            return NotFound();
        //    }
        //    else
        //    {
        //        ViewBag.ErrorTitle = "Null or Zero ID";
        //        ViewBag.ErrorMessage = "Null or zero ID in Delete method of Dept controller";
        //        return View("ErrorView");
        //    }
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(DeptVM depVM)
        {
            if (depVM == null)
            {
                ViewBag.ErrorTitle = "Null Dept";
                ViewBag.ErrorMessage = "Null Dept found in View model";
                return View("ErrorView");
            }

            _uow.Dept.Remove(depVM.Dept);
            _uow.Save();
            TempData["success"] = "Dept deleted successfully";
            return RedirectToAction("Index");

        }


        #region API


        // DELETE: DeptController/RemoveDept/5
        [HttpDelete]
        public IActionResult RemoveDept(int ID)
        {

            var dept = _uow.Dept.GetOne(u => u.ID == ID);
            if (dept == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _uow.Dept.Remove(dept);
            _uow.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion






    }
}
