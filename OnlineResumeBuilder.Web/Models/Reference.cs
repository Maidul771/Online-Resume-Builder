using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Reference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Referee Name")]
        public string RefereeName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Organization")]
        public string Organization { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Display(Name = "Relationship")]
        public string? Relationship { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }
} 