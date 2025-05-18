using System.ComponentModel.DataAnnotations;

namespace OnlineResumeBuilder.Web.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "GitHub Link")]
        [Url]
        public string? GitHubLink { get; set; }

        [Display(Name = "LinkedIn Link")]
        [Url]
        public string? LinkedInLink { get; set; }

        // Navigation properties
        public virtual ICollection<Objective>? Objectives { get; set; }
        public virtual ICollection<Skill>? Skills { get; set; }
        public virtual ICollection<Experience>? Experiences { get; set; }
        public virtual ICollection<Project>? Projects { get; set; }
        public virtual ICollection<Education>? Educations { get; set; }
        public virtual ICollection<Certification>? Certifications { get; set; }
        public virtual ICollection<Reference>? References { get; set; }
    }
} 