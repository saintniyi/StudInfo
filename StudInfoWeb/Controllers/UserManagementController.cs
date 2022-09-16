using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoDataAccess.IRepository;
using StudInfoDataAccess.Repository;
using StudInfoModel;
using StudInfoModel.ViewModels;
using System.Security.Claims;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;


        public UserManagementController(IUnitOfWork uow, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _uow = uow;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        //_userManager.AddToRoleAsync(user, "Admin").Wait();
        //await _userManager.IsInRoleAsync(user, role.Name)
        //var claimsIdentity = (ClaimsIdentity)User.Identity
        //var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentitfier);
        //var username = HttpContext.Current.User.Identity.Name;
        //var id = _userManager.GetUserId(User); // Get user id:

                

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var userList = _uow.AppUser.GetAll().Select(x => new AppUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.Email
            });

            return Json( new { data = userList } );
        }


        public async Task<IActionResult> Edit(string id)
        {
            var user = _uow.AppUser.GetOne(x => x.Id == id);
            var roles = _roleManager.Roles.ToList();
           
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = new List<SelectListItem>();

            //foreach (var role in roles)
            //{
            //    var hasRole = userRoles.ToList().Contains(role.Name);
            //    roleItems.Add(new SelectListItem(role.Name, role.Id, hasRole));
            //}

            roleItems = roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id,
                Selected = userRoles.Contains(x.Name),
            }).ToList();
            
            UserRolesVM uvm = new()
            {
                AppUser = user,
                RoleList = roleItems
            };

            return View(uvm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserRolesVM userRolesVM)
        {
            //var user = _uow.AppUser.GetOne(x => x.Id == userRolesVM.AppUser.Id);

            if (userRolesVM.AppUser.Id == null)
            {
                return NotFound();
            }

            var user = userRolesVM.AppUser;

            //get existing user roles from the db
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            //delete existing user roles from db
            await _signInManager.UserManager.RemoveFromRolesAsync(user, userRoles);

            var roleItems = userRolesVM.RoleList;        //new List<SelectListItem>();
            List<string> roleNames = new List<string>();
                        
            foreach(var role in roleItems)
            {
                if (role.Selected)
                {
                    roleNames.Add(role.Text);
                }
            }

            //save userroles here
            await _signInManager.UserManager.AddToRolesAsync(user, roleNames);

            _uow.AppUser.Update(user);
            _uow.Save();



            TempData["success"] = "User updated successfully";

            //return RedirectToAction("Edit", new { id = user.Id });

            return RedirectToAction("Index");   
        }






    }
}


