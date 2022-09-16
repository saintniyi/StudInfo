using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace StudInfoModel
{
       
    public class Student
    {
        public int Id { get; set; }


        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "Student Nos")]
        public string StudNos { get; set; }



        [Required]
        [Display(Name = "First Name")]
        [StringLength(80)]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        [StringLength(80)]
        public string LastName { get; set; }


        [Required]
        [StringLength(90)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Phone")]
        public string PhoneNos { get; set; }


        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DOB { get; set; }


        [Required]
        [Display(Name = "Admission Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime AdmissionDate { get; set; }



        [Required]
        public Gender Sex  { get; set; }


        [Display(Name = "Picture")]
        public byte[]? Pix { get; set; }


        [Required]
        [Display(Name = "Dept")]
        public string DeptCode { get; set; }


        [ForeignKey("DeptCode")]
        [ValidateNever]
        public Dept? Dept { get; set; }



        [Required]
        [Display(Name = "Degree Program Name")]
        public ProgramName DegreeProgramName { get; set; }


        [Required]
        [Display(Name = "Degree Title")]
        public DegreeTitle DegreeTitleToBeAwarded { get; set; }


        public int? Level { get; set; }


        public int? Semester { get; set; }


        [StringLength(20)]
        public string? LatestEnrollGrpCode { get; set; }


      
        public float? GPA { get; set; }


        
        public float? CGPA { get; set; }


        [StringLength(500)]
        public string? Remarks { get; set; }


        [StringLength(3000)]
        public string? PhotoBackupPath { get; set; }


    }



#region Enums


    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    
    public enum ProgramName
    {
        Accounting = 1,

        [Display(Name = "Business Administration")]
        BusAdmin = 2,

        [Display(Name = "Biological Science")]
        BioScience = 3,

        [Display(Name = "Computer Science")]
        ComputerScience = 4,

        [Display(Name = "Electrical Electronics Engineering")]
        ElectElect = 5,


        Mathematics = 6,


        [Display(Name = "Medicine and Surgery")]
        MedSurgery = 7
    }



    public enum DegreeTitle {
        [Display(Name = "Bachelor of Art")]
        BA = 1,

        [Display(Name = "Bachelor of Engineering")]
        BEng = 2,

        [Display(Name = "Bachelor of Science")]
        BSc = 3,

        [Display(Name = "Bachelor of Medicine and Bachelor of Surgery")]
        MBBS = 4
    }


    

    #endregion Enums









}
