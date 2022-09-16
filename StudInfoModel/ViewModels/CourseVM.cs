using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel.ViewModels
{
    public class CourseVM
    {

        public Course Course { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>?  DeptList { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>? SemesterList { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>? LevelList { get; set; }

    }
}
