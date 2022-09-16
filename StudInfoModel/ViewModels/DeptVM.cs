using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudInfoModel;

namespace StudInfoModel.ViewModels
{
    public class DeptVM
    {
        public Dept Dept { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? FacultyList { get; set; }
    }
}
