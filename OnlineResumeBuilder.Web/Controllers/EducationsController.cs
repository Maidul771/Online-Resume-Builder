using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class EducationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EducationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Educations
        public async Task<IActionResult> Index()
        {
            var educations = await _context.Educations
                .Include(e => e.UserInfo)
                .ToListAsync();
            return View(educations);
        }

        // GET: Educations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // GET: Educations/Create
        public IActionResult Create()
        {
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
            ViewData["DegreeTypes"] = GetDegreeTypesSelectList();
            ViewData["SubjectTypes"] = GetSubjectTypesSelectList();
            return View();
        }

        // POST: Educations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Degree,Subject,Institution,GraduationYear,UserInfoId")] Education education)
        {
            if (ModelState.IsValid)
            {
                _context.Add(education);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", education.UserInfoId);
            ViewData["DegreeTypes"] = GetDegreeTypesSelectList(education.Degree);
            ViewData["SubjectTypes"] = GetSubjectTypesSelectList(education.Subject);
            return View(education);
        }

        // GET: Educations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", education.UserInfoId);
            ViewData["DegreeTypes"] = GetDegreeTypesSelectList(education.Degree);
            ViewData["SubjectTypes"] = GetSubjectTypesSelectList(education.Subject);
            return View(education);
        }

        // POST: Educations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Degree,Subject,Institution,GraduationYear,UserInfoId")] Education education)
        {
            if (id != education.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(education.Id))
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
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", education.UserInfoId);
            ViewData["DegreeTypes"] = GetDegreeTypesSelectList(education.Degree);
            ViewData["SubjectTypes"] = GetSubjectTypesSelectList(education.Subject);
            return View(education);
        }

        // GET: Educations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationExists(int id)
        {
            return _context.Educations.Any(e => e.Id == id);
        }

        private SelectList GetDegreeTypesSelectList(DegreeType selectedDegreeType = DegreeType.BSc)
        {
            var degreeTypes = Enum.GetValues(typeof(DegreeType))
                                 .Cast<DegreeType>()
                                 .Select(d => new
                                 {
                                     Value = d,
                                     Text = GetDisplayName(d)
                                 })
                                 .ToList();

            return new SelectList(degreeTypes, "Value", "Text", selectedDegreeType);
        }

        private SelectList GetSubjectTypesSelectList(SubjectType selectedSubjectType = SubjectType.ComputerScience)
        {
            var subjectTypes = Enum.GetValues(typeof(SubjectType))
                                 .Cast<SubjectType>()
                                 .Select(s => new
                                 {
                                     Value = s,
                                     Text = GetDisplayName(s)
                                 })
                                 .ToList();

            return new SelectList(subjectTypes, "Value", "Text", selectedSubjectType);
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