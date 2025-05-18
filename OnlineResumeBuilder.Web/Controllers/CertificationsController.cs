using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class CertificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Certifications
        public async Task<IActionResult> Index()
        {
            var certifications = await _context.Certifications
                .Include(c => c.UserInfo)
                .ToListAsync();
            return View(certifications);
        }

        // GET: Certifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certification = await _context.Certifications
                .Include(c => c.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certification == null)
            {
                return NotFound();
            }

            return View(certification);
        }

        // GET: Certifications/Create
        public IActionResult Create()
        {
            var users = _context.UserInfos.ToList();
            
            if (!users.Any())
            {
                // No users exist, redirect to create user
                TempData["ErrorMessage"] = "You must create a user profile before adding certifications.";
                return RedirectToAction("Create", "UserInfos");
            }
            
            if (users.Count == 1)
            {
                // If only one user exists, preselect it
                ViewData["UserInfoId"] = new SelectList(users, "Id", "FullName", users.First().Id);
            }
            else
            {
                ViewData["UserInfoId"] = new SelectList(users, "Id", "FullName");
            }
            
            return View();
        }

        // POST: Certifications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseName,Institute,CompletionYear,CertificateUrl,UserInfoId")] Certification certification)
        {
            if (ModelState.IsValid)
            {
                // Ensure UserInfoId refers to a valid user
                if (!_context.UserInfos.Any(u => u.Id == certification.UserInfoId))
                {
                    ModelState.AddModelError("UserInfoId", "Please select a valid user.");
                    ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
                    return View(certification);
                }
                
                _context.Add(certification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", certification.UserInfoId);
            return View(certification);
        }

        // GET: Certifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certification = await _context.Certifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound();
            }
            
            var users = _context.UserInfos.ToList();
            if (!users.Any())
            {
                TempData["ErrorMessage"] = "No users exist in the system.";
                return RedirectToAction("Index", "UserInfos");
            }
            
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", certification.UserInfoId);
            return View(certification);
        }

        // POST: Certifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,Institute,CompletionYear,CertificateUrl,UserInfoId")] Certification certification)
        {
            if (id != certification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Ensure UserInfoId refers to a valid user
                if (!_context.UserInfos.Any(u => u.Id == certification.UserInfoId))
                {
                    ModelState.AddModelError("UserInfoId", "Please select a valid user.");
                    ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
                    return View(certification);
                }
                
                try
                {
                    _context.Update(certification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificationExists(certification.Id))
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
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", certification.UserInfoId);
            return View(certification);
        }

        // GET: Certifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certification = await _context.Certifications
                .Include(c => c.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certification == null)
            {
                return NotFound();
            }

            return View(certification);
        }

        // POST: Certifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certification = await _context.Certifications.FindAsync(id);
            if (certification != null)
            {
                _context.Certifications.Remove(certification);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificationExists(int id)
        {
            return _context.Certifications.Any(e => e.Id == id);
        }
    }
} 