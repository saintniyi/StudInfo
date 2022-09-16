using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoModel.ViewModels;

namespace StudInfoWeb.Controllers
{

    [Authorize]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHost;

        public StudentController(IUnitOfWork uow, IWebHostEnvironment webHost)
        {
            _uow = uow;
            _webHost = webHost;
        }


        public IActionResult Index()
        {
            return View();
        }



        // GET: StudentController/Upsert/id
        public IActionResult Upsert(int? id)
        {
            StudentVM studentVM = new();
            studentVM.Student = new();
            studentVM.DeptList = _uow.Dept.GetAll().Select(x => new SelectListItem
            {
                Text = x.DeptName,
                Value = x.DeptCode
            });


            if (id == null || id == 0)
            {
                //this is an insert mode, id is null or zero
                return View(studentVM);
            }
            else
            {
                //this is an update mode, id must have a value
                studentVM.Student = _uow.Student.GetOne(x => x.Id == id);

                string imageBase64Data = Convert.ToBase64String(studentVM.Student.Pix);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageDataUrl = imageDataURL;

                return View(studentVM);
            }

        }


        // POST: StudentController/Upsert/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StudentVM studentVM)
        {
            if (ModelState.IsValid)
            {
                if (studentVM.Student.Id == 0)
                {

                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];

                        //convert file to byte array
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            studentVM.Student.Pix = ms.ToArray();
                        }

                        //upload file to image/upload folder and returns filepath
                        string filepath = uploadFile(Request.Form.Files[0]);


                        studentVM.Student.PhotoBackupPath = filepath;

                    }

                    //this is an insert operation
                    _uow.Student.Add(studentVM.Student);
                    TempData["success"] = "Student created successfully";
                }
                else
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        if (!string.IsNullOrEmpty(studentVM.Student.PhotoBackupPath))
                        {
                            string existingFilePath = studentVM.Student.PhotoBackupPath;
                            System.IO.File.Delete(existingFilePath);
                        }

                        //upload file to image/upload folder and returns new filepath
                        string filepath = uploadFile(Request.Form.Files[0]);

                        //convert file to byte array
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            studentVM.Student.Pix = ms.ToArray();
                        }

                        studentVM.Student.PhotoBackupPath = filepath;
                    }
                    else
                    {
                        var stud = _uow.Student.GetOne(x => x.Id == studentVM.Student.Id);
                        studentVM.Student.Pix = stud.Pix;
                        stud = null;
                    }

                    //this is an update operation
                    _uow.Student.Update(studentVM.Student);
                    TempData["success"] = "Student updated successfully";
                }

                _uow.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View("studentVM");
            }

        }

        private string uploadFile(IFormFile file)
        {
            string filepath = string.Empty;

            if (file != null)
            {
                string uploadFolder = Path.Combine(_webHost.WebRootPath, "images\\upload");
                string uniqueFilename = Guid.NewGuid().ToString() + file.FileName;
                filepath = Path.Combine(uploadFolder, uniqueFilename);

                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
            }

            return filepath;
        }



        // GET: StudentController/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                StudentVM studentVM = new();
                studentVM.Student = _uow.Student.GetOne(x => x.Id == id);
                studentVM.DeptList = _uow.Dept.GetAll().Select(x => new SelectListItem
                {
                    Text = x.DeptName,
                    Value = x.DeptCode
                });


                if (studentVM.Student != null)
                {
                    string imageBase64Data = Convert.ToBase64String(studentVM.Student.Pix);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    ViewBag.ImageDataUrl = imageDataURL;

                    return View(studentVM);
                }
                else
                    return NotFound();
            }
            else
            {
                ViewBag.ErrorTitle = "Null or Zero ID";
                ViewBag.ErrorMessage = "Null or zero ID in Delete method of Student controller";
                return View("ErrorView");
            }
        }



        #region API CALLS


        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var studList = _uow.Student.GetAll();
            return Json(new { data = studList });
        }


        // DELETE: StudentController/RemoveStudent/5
        [HttpDelete]
        public IActionResult RemoveStudent(int id)
        {

            var student = _uow.Student.GetOne(x => x.Id == id);
            if (student == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (student != null)
            {
                if (!string.IsNullOrEmpty(student.PhotoBackupPath))
                {
                    string existingFilePath = student.PhotoBackupPath;
                    System.IO.File.Delete(existingFilePath);
                }

                _uow.Student.Remove(student);
            }

            _uow.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion




    }
}
