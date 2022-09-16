using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel
{
    public class Grade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Key]
        [Required]
        [StringLength(5)]
        [Display(Name = "Grade Letter")]
        public string GradeLetter { get; set; }


        [Required]
        [Display(Name = "Minimum Score")]
        public float MinScoreRange { get; set; }


        [Required]
        [Display(Name = "Maximum Score")]
        public float MaxScoreRange { get; set; }
                


        [Required]
        public float Weight { get; set; }



    }
}
