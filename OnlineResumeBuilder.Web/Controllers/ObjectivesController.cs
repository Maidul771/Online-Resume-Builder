using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Data;
using OnlineResumeBuilder.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineResumeBuilder.Web.Controllers
{
    public class ObjectivesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObjectivesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Objectives
        public async Task<IActionResult> Index()
        {
            var objectives = await _context.Objectives
                .Include(o => o.UserInfo)
                .ToListAsync();
            return View(objectives);
        }

        // GET: Objectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives
                .Include(o => o.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (objective == null)
            {
                return NotFound();
            }

            return View(objective);
        }

        // GET: Objectives/Create
        public IActionResult Create()
        {
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName");
            return View();
        }

        // POST: Objectives/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details,UserInfoId")] Objective objective)
        {
            if (ModelState.IsValid)
            {
                _context.Add(objective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", objective.UserInfoId);
            return View(objective);
        }

        // GET: Objectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives.FindAsync(id);
            if (objective == null)
            {
                return NotFound();
            }
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", objective.UserInfoId);
            return View(objective);
        }

        // POST: Objectives/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details,UserInfoId")] Objective objective)
        {
            if (id != objective.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objective);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectiveExists(objective.Id))
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
            ViewData["UserInfoId"] = new SelectList(_context.UserInfos, "Id", "FullName", objective.UserInfoId);
            return View(objective);
        }

        // GET: Objectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objective = await _context.Objectives
                .Include(o => o.UserInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (objective == null)
            {
                return NotFound();
            }

            return View(objective);
        }

        // POST: Objectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objective = await _context.Objectives.FindAsync(id);
            if (objective != null)
            {
                _context.Objectives.Remove(objective);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObjectiveExists(int id)
        {
            return _context.Objectives.Any(e => e.Id == id);
        }
    }
} 