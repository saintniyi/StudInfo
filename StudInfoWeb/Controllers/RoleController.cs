using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using System.Data;

namespace StudInfoWeb.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetRolesList()
        {
            var roleManagerList = _roleManager.Roles.ToList()
                .OrderBy(x => x.Name);

            var roleList = roleManagerList.Select(x => new Role
            {
                Id = x.Id,
                RoleName = x.Name
            }).ToList();

            return Json(new { data = roleList });
        }



        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            var role = new Role();
            if (string.IsNullOrEmpty(id))
            {
                //this is an insert mode
                
                return View(role);
            }
            else
            {
                //this is an update mode, ID must have a value
                var result = await _roleManager.FindByIdAsync(id);

                role = new Role()
                {
                    Id = result.Id,
                    RoleName = result.Name
                };

                return View(role);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Role role)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(role.Id))
                {
                    //this is an insert operation
                    IdentityRole identityRole = new IdentityRole()
                    {
                        Name = role.RoleName
                    };

                    var result = await _roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        TempData["success"] = $"{role.RoleName} role created successfully";
                    }

                    RedirectToAction("Index");

                }
                else
                {
                    //this is an update operation, get an handle of the specific rolw
                    var foundRole = await _roleManager.FindByIdAsync(role.Id);

                    if (foundRole != null)
                    {
                        foundRole.Name = role.RoleName;
                        await _roleManager.UpdateAsync(foundRole);
                    }

                    TempData["success"] = "role updated successfully";
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View("role");
            }

        }
                


        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var foundRole = await _roleManager.FindByIdAsync(id);

            if (foundRole == null)
            {
                return Json(new { sno = 1, message = "role could not be found" });
            }

            //get users attached to role
            var lstUsers = await _userManager.GetUsersInRoleAsync(foundRole.Name);

            if (lstUsers.Any())
            {
                foreach (var user in lstUsers)
                {
                    await _userManager.RemoveFromRoleAsync(user, foundRole.Name);
                }

                var result = await _roleManager.DeleteAsync(foundRole);

                if (result.Succeeded)
                {
                    return Json(new { sno = 2, message = "role successfully deleted" });
                }
                else
                {
                    return Json(new { sno = 3, message = "role delete fails" });
                }
            }


            return Json(new { sno = 4, message = "" });





        }









    }
}
