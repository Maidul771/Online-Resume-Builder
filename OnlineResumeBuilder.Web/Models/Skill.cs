using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Skill Name")]
        public string SkillName { get; set; } = string.Empty;

        [Display(Name = "Skill Type")]
        public SkillType SkillType { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }

    public enum SkillType
    {
        [Display(Name = ".NET Core")]
        DotNetCore,
        [Display(Name = "ASP.NET MVC")]
        AspNetMvc,
        [Display(Name = "Entity Framework")]
        EntityFramework,
        [Display(Name = "SQL Server")]
        SqlServer,
        [Display(Name = "C#")]
        CSharp,
        [Display(Name = "Azure")]
        Azure,
        [Display(Name = "Web API")]
        WebApi,
        [Display(Name = "Blazor")]
        Blazor,
        [Display(Name = "Other")]
        Other
    }
} 