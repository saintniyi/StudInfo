using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel
{
    public class Faculty
    {
        [Key]
        [Required]
        [Display(Name = "Faculty Code")]
        //StringLength(3, ErrorMessage = "Faculty Code cannot be greater than 3 characters", MinimumLength = 3)]
        [StringLength(50, ErrorMessage = "Faculty Code cannot be greater than 50 characters")]
        public string FacultyCode { get; set; }

        [Required]
        [Display(Name = "Faculty Name")]
        [StringLength(120, ErrorMessage = "Faculty Name cannot be more 120 character")]
        public string FacultyName { get; set; }
    }
}
