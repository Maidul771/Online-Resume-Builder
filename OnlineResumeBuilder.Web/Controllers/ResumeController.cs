using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class ResumeController : Controller
    {
        // Static constructor to initialize QuestPDF license once for the entire application
        static ResumeController()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private readonly ApplicationDbContext _context;

        public ResumeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resume/Preview
        public async Task<IActionResult> Preview(int? id)
        {
            // If no ID is specified, get the first user or show an empty page
            var userInfoId = id ?? await _context.UserInfos.Select(u => u.Id).FirstOrDefaultAsync();
            
            if (userInfoId == 0)
            {
                return View(new UserInfo());
            }

            var userInfo = await _context.UserInfos
                .Include(u => u.Objectives)
                .Include(u => u.Skills)
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Educations)
                .Include(u => u.Certifications)
                .Include(u => u.References)
                .FirstOrDefaultAsync(u => u.Id == userInfoId);

            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: Resume/Download
        public async Task<IActionResult> Download(int? id)
        {
            // If no ID is specified, get the first user or show an empty page
            var userInfoId = id ?? await _context.UserInfos.Select(u => u.Id).FirstOrDefaultAsync();
            
            if (userInfoId == 0)
            {
                return RedirectToAction("Index", "UserInfos");
            }

            var userInfo = await _context.UserInfos
                .Include(u => u.Objectives)
                .Include(u => u.Skills)
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Educations)
                .Include(u => u.Certifications)
                .Include(u => u.References)
                .FirstOrDefaultAsync(u => u.Id == userInfoId);

            if (userInfo == null)
            {
                return NotFound();
            }

            var pdfDocument = GeneratePdf(userInfo);
            var pdfBytes = pdfDocument.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Resume-{userInfo.FullName}.pdf");
        }

        // View for Download page
        public async Task<IActionResult> DownloadPage(int? id)
        {
            // If no ID is specified, get the first user or show an empty page
            var userInfoId = id ?? await _context.UserInfos.Select(u => u.Id).FirstOrDefaultAsync();
            
            if (userInfoId == 0)
            {
                return RedirectToAction("Index", "UserInfos");
            }

            var userInfo = await _context.UserInfos
                .Include(u => u.Objectives)
                .Include(u => u.Skills)
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Educations)
                .Include(u => u.Certifications)
                .Include(u => u.References)
                .FirstOrDefaultAsync(u => u.Id == userInfoId);

            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        private Document GeneratePdf(UserInfo userInfo)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    
                    // Only show header on the first page
                    page.Header().ShowOnce().Element(header =>
                    {
                        header.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text(userInfo.FullName).FontSize(20).Bold();
                                
                                column.Item().Text(text =>
                                {
                                    text.Span("Email: ").Bold();
                                    text.Span(userInfo.Email);
                                });
                                
                                if (!string.IsNullOrEmpty(userInfo.PhoneNumber))
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Phone: ").Bold();
                                        text.Span(userInfo.PhoneNumber);
                                    });
                                }
                                
                                if (!string.IsNullOrEmpty(userInfo.GitHubLink))
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("GitHub: ").Bold();
                                        text.Span(userInfo.GitHubLink);
                                    });
                                }
                                
                                if (!string.IsNullOrEmpty(userInfo.LinkedInLink))
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("LinkedIn: ").Bold();
                                        text.Span(userInfo.LinkedInLink);
                                    });
                                }
                            });
                        });
                    });

                    page.Content().Column(mainContent =>
                    {
                        // Objective Section
                        if (userInfo.Objectives != null && userInfo.Objectives.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("OBJECTIVE").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var objective in userInfo.Objectives)
                                {
                                    column.Item().Text(objective.Details);
                                }
                                
                                column.Spacing(10);
                            });
                        }

                        // Skills Section
                        if (userInfo.Skills != null && userInfo.Skills.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("SKILLS").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                column.Item().Text(string.Join(", ", userInfo.Skills.Select(s => s.SkillName)));
                                
                                column.Spacing(10);
                            });
                        }

                        // Experience Section
                        if (userInfo.Experiences != null && userInfo.Experiences.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("PROFESSIONAL EXPERIENCE").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var experience in userInfo.Experiences.OrderByDescending(e => e.StartDate))
                                {
                                    column.Item().Column(expColumn =>
                                    {
                                        expColumn.Spacing(5);
                                        expColumn.Item().Text(text =>
                                        {
                                            text.Span($"{experience.CompanyName} - {experience.Position}").Bold();
                                            text.Span($" ({experience.StartDate:MMM yyyy} - {(experience.EndDate.HasValue ? experience.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                                        });

                                        expColumn.Item().Text(experience.Details);
                                    });
                                    
                                    column.Spacing(5);
                                }
                                
                                column.Spacing(10);
                            });
                        }

                        // Projects Section
                        if (userInfo.Projects != null && userInfo.Projects.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("PROJECTS").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var project in userInfo.Projects)
                                {
                                    column.Item().Column(projColumn =>
                                    {
                                        projColumn.Spacing(5);
                                        projColumn.Item().Text(project.ProjectName).Bold();
                                        projColumn.Item().Text(project.Description);
                                        
                                        var techFlags = Enum.GetValues(typeof(TechnologyType))
                                            .Cast<TechnologyType>()
                                            .Where(t => project.TechnologyUsed.HasFlag(t) && t != TechnologyType.None)
                                            .Select(t => GetDisplayName(t));
                                        
                                        if (techFlags.Any())
                                        {
                                            projColumn.Item().Text($"Technologies: {string.Join(", ", techFlags)}");
                                        }
                                    });
                                    
                                    column.Spacing(5);
                                }
                                
                                column.Spacing(10);
                            });
                        }

                        // Education Section
                        if (userInfo.Educations != null && userInfo.Educations.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("EDUCATION").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var education in userInfo.Educations.OrderByDescending(e => e.GraduationYear))
                                {
                                    column.Item().Column(eduColumn =>
                                    {
                                        eduColumn.Spacing(5);
                                        eduColumn.Item().Text(text =>
                                        {
                                            text.Span($"{GetDisplayName(education.Degree)} in {GetDisplayName(education.Subject)}").Bold();
                                            if (education.GraduationYear.HasValue)
                                                text.Span($" ({education.GraduationYear})");
                                        });
                                        
                                        eduColumn.Item().Text(education.Institution);
                                    });
                                    
                                    column.Spacing(5);
                                }
                                
                                column.Spacing(10);
                            });
                        }

                        // Certifications Section
                        if (userInfo.Certifications != null && userInfo.Certifications.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("CERTIFICATIONS").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var certification in userInfo.Certifications)
                                {
                                    column.Item().Column(certColumn =>
                                    {
                                        certColumn.Spacing(5);
                                        certColumn.Item().Text(text =>
                                        {
                                            text.Span(certification.CourseName).Bold();
                                            if (certification.CompletionYear.HasValue)
                                                text.Span($" ({certification.CompletionYear})");
                                        });
                                        
                                        certColumn.Item().Text(certification.Institute);
                                    });
                                    
                                    column.Spacing(5);
                                }
                                
                                column.Spacing(10);
                            });
                        }

                        // References Section
                        if (userInfo.References != null && userInfo.References.Any())
                        {
                            mainContent.Item().Column(column =>
                            {
                                column.Item().BorderBottom(1).PaddingBottom(5).Text("REFERENCES").Bold().FontSize(14);
                                column.Spacing(5);
                                
                                foreach (var reference in userInfo.References)
                                {
                                    column.Item().Column(refColumn =>
                                    {
                                        refColumn.Spacing(5);
                                        refColumn.Item().Text(reference.RefereeName).Bold();
                                        refColumn.Item().Text(reference.Organization);
                                        refColumn.Item().Text($"Email: {reference.Email}");
                                        if (!string.IsNullOrEmpty(reference.Phone))
                                            refColumn.Item().Text($"Phone: {reference.Phone}");
                                    });
                                    
                                    column.Spacing(5);
                                }
                            });
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ").FontSize(10);
                            x.CurrentPageNumber().FontSize(10);
                            x.Span(" of ").FontSize(10);
                            x.TotalPages().FontSize(10);
                        });
                });
            });
        }

        private string GetDisplayName<T>(T enumValue) where T : Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var displayAttribute = (System.ComponentModel.DataAnnotations.DisplayAttribute)fieldInfo?
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                .FirstOrDefault();
            
            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
} 