using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel.ViewModels
{
    public class StudentVM
    {

        public Student Student { get; set; } = null!;

        public IFormFile? Photo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem>?  DeptList { get; set; }



    }
}
