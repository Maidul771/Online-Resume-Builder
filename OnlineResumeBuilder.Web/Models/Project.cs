using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResumeBuilder.Web.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Technology Used")]
        public TechnologyType TechnologyUsed { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 words")]
        [Display(Name = "Project Description")]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo? UserInfo { get; set; }
    }

    [Flags]
    public enum TechnologyType
    {
        None = 0,
        [Display(Name = ".NET Core")]
        DotNetCore = 1,
        [Display(Name = "Entity Framework")]
        EntityFramework = 2,
        [Display(Name = "SQL Server")]
        SqlServer = 4,
        [Display(Name = "ASP.NET MVC")]
        AspNetMvc = 8,
        [Display(Name = "Web API")]
        WebApi = 16,
        [Display(Name = "Blazor")]
        Blazor = 32,
        [Display(Name = "Azure")]
        Azure = 64,
        [Display(Name = "Other")]
        Other = 128
    }
} 