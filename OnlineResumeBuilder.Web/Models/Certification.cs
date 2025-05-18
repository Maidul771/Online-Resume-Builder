using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Certification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Institute")]
        public string Institute { get; set; } = string.Empty;

        [Display(Name = "Completion Year")]
        public int? CompletionYear { get; set; }

        [Display(Name = "Certificate URL")]
        [Url]
        public string? CertificateUrl { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }
} 