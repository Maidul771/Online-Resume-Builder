using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class ReferencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReferencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: References
        public async Task<IActionResult> Index()
        {
            var references = await _context.References
                .Include(r => r.UserInfo)
                .ToListAsync();
            return View(references);
        }

        // GET: References/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reference = await _context.References
                .Include(r => r.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reference == null)
            {
                return NotFound();
            }

            return View(reference);
        }

        // GET: References/Create
        public IActionResult Create()
        {
            var users = _context.UserInfos.ToList();
            
            if (!users.Any())
            {
                // No users exist, redirect to create user
                TempData["ErrorMessage"] = "You must create a user profile before adding references.";
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

        // POST: References/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RefereeName,Organization,Email,Phone,Relationship,UserInfoId")] Reference reference)
        {
            if (ModelState.IsValid)
            {
                // Ensure UserInfoId refers to a valid user
                if (!_context.UserInfos.Any(u => u.Id == reference.UserInfoId))
                {
                    ModelState.AddModelError("UserInfoId", "Please select a valid user.");
                    ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
                    return View(reference);
                }
                
                _context.Add(reference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", reference.UserInfoId);
            return View(reference);
        }

        // GET: References/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reference = await _context.References.FindAsync(id);
            if (reference == null)
            {
                return NotFound();
            }
            
            var users = _context.UserInfos.ToList();
            if (!users.Any())
            {
                TempData["ErrorMessage"] = "No users exist in the system.";
                return RedirectToAction("Index", "UserInfos");
            }
            
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", reference.UserInfoId);
            return View(reference);
        }

        // POST: References/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RefereeName,Organization,Email,Phone,Relationship,UserInfoId")] Reference reference)
        {
            if (id != reference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Ensure UserInfoId refers to a valid user
                if (!_context.UserInfos.Any(u => u.Id == reference.UserInfoId))
                {
                    ModelState.AddModelError("UserInfoId", "Please select a valid user.");
                    ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
                    return View(reference);
                }
                
                try
                {
                    _context.Update(reference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferenceExists(reference.Id))
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
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", reference.UserInfoId);
            return View(reference);
        }

        // GET: References/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reference = await _context.References
                .Include(r => r.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reference == null)
            {
                return NotFound();
            }

            return View(reference);
        }

        // POST: References/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reference = await _context.References.FindAsync(id);
            if (reference != null)
            {
                _context.References.Remove(reference);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferenceExists(int id)
        {
            return _context.References.Any(e => e.Id == id);
        }
    }
} 