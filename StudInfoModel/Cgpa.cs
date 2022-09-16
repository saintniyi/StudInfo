using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudInfoModel
{
    public class Cgpa
    {
        [Key]
        public int Id { get; set; }



        [Required]
        [StringLength(50)]
        [Display(Name = "Student Nos")]
        public string? StudNos { get; set; }



        public int? TotalUnits { get; set; }



        public float? TotalUnitWeight { get; set; }



        public float? CumulativeGPA { get; set; }





    }
}
