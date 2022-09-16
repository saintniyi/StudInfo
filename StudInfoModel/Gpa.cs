using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudInfoModel
{
    public class Gpa
    {
        [Key]
        public int Id { get; set; }

                

        [Required]
        [StringLength(50)]
        [Display(Name = "Student Nos")]
        public string? StudNos { get; set; }



        [Required]
        public int? Level { get; set; }



        [Required]
        public int? Semester { get; set; }


        public int? TotalSemesterUnits { get; set; }



        public float? TotalSemesterUnitWeight { get; set; }



        public float? SemesterGPA { get; set; }





    }
}
