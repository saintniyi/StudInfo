using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel.ViewModels
{
    public class UserRolesVM
    {
        public AppUser AppUser { get; set; }

        public List<SelectListItem> RoleList { get; set; }

    }
}
