using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.UserInfo)
                .ToListAsync();
            return View(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
            ViewData["TechnologyTypes"] = GetTechnologyTypesCheckboxList();
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,TechnologyUsed,Description,UserInfoId")] Project project, int[] selectedTechnologies = null)
        {
            // If selectedTechnologies were passed directly, use them to set TechnologyUsed
            if (selectedTechnologies != null && selectedTechnologies.Length > 0)
            {
                project.TechnologyUsed = 0; // Reset the value
                foreach (var tech in selectedTechnologies)
                {
                    project.TechnologyUsed |= (TechnologyType)tech;
                }
                // Ensure ModelState is valid for TechnologyUsed
                ModelState.Remove("TechnologyUsed");
            }

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", project.UserInfoId);
            ViewData["TechnologyTypes"] = GetTechnologyTypesCheckboxList(project.TechnologyUsed);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", project.UserInfoId);
            ViewData["TechnologyTypes"] = GetTechnologyTypesCheckboxList(project.TechnologyUsed);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,TechnologyUsed,Description,UserInfoId")] Project project, int[] selectedTechnologies = null)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            // If selectedTechnologies were passed directly, use them to set TechnologyUsed
            if (selectedTechnologies != null && selectedTechnologies.Length > 0)
            {
                project.TechnologyUsed = 0; // Reset the value
                foreach (var tech in selectedTechnologies)
                {
                    project.TechnologyUsed |= (TechnologyType)tech;
                }
                // Ensure ModelState is valid for TechnologyUsed
                ModelState.Remove("TechnologyUsed");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", project.UserInfoId);
            ViewData["TechnologyTypes"] = GetTechnologyTypesCheckboxList(project.TechnologyUsed);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        private List<CheckBoxItem> GetTechnologyTypesCheckboxList(TechnologyType selectedTechnologyTypes = 0)
        {
            var techTypes = Enum.GetValues(typeof(TechnologyType))
                                .Cast<TechnologyType>()
                                .Select(t => new CheckBoxItem
                                {
                                    Id = (int)t,
                                    Text = GetDisplayName(t),
                                    IsChecked = (selectedTechnologyTypes & t) == t
                                })
                                .ToList();

            return techTypes;
        }

        private string GetDisplayName(TechnologyType techType)
        {
            var fieldInfo = techType.GetType().GetField(techType.ToString());
            var displayAttribute = (System.ComponentModel.DataAnnotations.DisplayAttribute)fieldInfo?
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                .FirstOrDefault();
            
            return displayAttribute?.Name ?? techType.ToString();
        }
    }

    public class CheckBoxItem
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }
} 