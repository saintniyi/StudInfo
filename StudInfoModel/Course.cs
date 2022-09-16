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
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }



        [Key]
        [Required]
        [Display(Name = "Course Code")]
        [StringLength(50, ErrorMessage = "Course Code cannot be more than 50 characters")]
        public string CourseCode { get; set; }



        [Required]
        [Display(Name = "Course Name")]
        [StringLength(120, ErrorMessage = "Course Name cannot be more than 120 characters")]
        public string CourseName { get; set; }



        [Required]
        [Display(Name = "Course Unit")]
        public int CourseUnit { get; set; }



        [Required]
        [Display(Name = "Dept")]
        public string DeptCode { get; set; }



        [Required]
        public int Level { get; set; }



        [Required]
        public int Semester { get; set; }



        [ForeignKey("DeptCode")]
        [ValidateNever]
        public Dept? Dept  { get; set; }
       
    }
}
