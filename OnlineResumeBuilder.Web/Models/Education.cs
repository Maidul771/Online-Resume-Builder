using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Degree")]
        public DegreeType Degree { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public SubjectType Subject { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public string Institution { get; set; } = string.Empty;

        [Display(Name = "Graduation Year")]
        public int? GraduationYear { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }

    public enum DegreeType
    {
        [Display(Name = "Bachelor of Science")]
        BSc,
        [Display(Name = "Master of Science")]
        MSc,
        [Display(Name = "PhD")]
        PhD,
        [Display(Name = "Bachelor of Arts")]
        BA,
        [Display(Name = "Master of Arts")]
        MA,
        [Display(Name = "Bachelor of Technology")]
        BTech,
        [Display(Name = "Master of Technology")]
        MTech,
        [Display(Name = "Diploma")]
        Diploma,
        [Display(Name = "Other")]
        Other
    }

    public enum SubjectType
    {
        [Display(Name = "Computer Science")]
        ComputerScience,
        [Display(Name = "Information Technology")]
        InformationTechnology,
        [Display(Name = "Electrical Engineering")]
        ElectricalEngineering,
        [Display(Name = "Electronics Engineering")]
        ElectronicsEngineering,
        [Display(Name = "Mechanical Engineering")]
        MechanicalEngineering,
        [Display(Name = "Civil Engineering")]
        CivilEngineering,
        [Display(Name = "Other")]
        Other
    }
} 