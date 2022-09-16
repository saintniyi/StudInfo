using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel.ViewModels
{
    public class EnrollmentVM
    {
        public Enrollment Enrollment { get; set; }

        public List<Enrollment>? Enrollments { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem>? CourseList { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>? StudentList { get; set; }


    }
}
