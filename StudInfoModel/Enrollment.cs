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
    public class Enrollment
    {
        [Key]
        public int ID { get; set; }



        [Required]
        [Display(Name = "Group Code")]
        [StringLength(20, ErrorMessage = "Group Code is required and must be same for a batch")]
        public string GroupCode { get; set; }



        [Required]
        [StringLength(50)]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Student Nos")]
        public string StudNos { get; set; }


        [Required]
        public int Level { get; set; }


        [Required]
        public int Semester { get; set; }



        [StringLength(1)]
        public string? Processed { get; set; }


        [ValidateNever]
        [ForeignKey("CourseCode")]
        public Course? Course { get; set; }


        [ValidateNever]
        [ForeignKey("StudNos")]
        public Student? Student { get; set; }


        


    }
}
