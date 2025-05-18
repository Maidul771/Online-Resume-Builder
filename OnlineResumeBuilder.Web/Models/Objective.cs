using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Objective
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "Objective must be at least 50 words")]
        [MaxLength(300, ErrorMessage = "Objective cannot exceed 300 words")]
        [Display(Name = "Career Objective")]
        public string Details { get; set; } = string.Empty;

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }
} 