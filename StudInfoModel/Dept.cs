using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel
{
    public class Dept
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Required]
        [StringLength(50, ErrorMessage = "Dept Code cannot exceed 50 characters")]
        [Display(Name = "Dept Code")]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "Dept Name cannot exceed 120 characters")]
        [Display(Name = "Dept Name")]
        public string DeptName { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public string FacultyCode { get; set; }

        [ForeignKey("FacultyCode")]
        [ValidateNever]
        public Faculty? Faculty { get; set; }
    }
}
