using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models;
using Microsoft.AspNetCore.Identity;

namespace Assignment2.Controllers
{
    public class LearningOutcomeNotesController : Controller
    {
        private readonly LOContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserRolesDB _userContext;

        public LearningOutcomeNotesController(LOContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, UserRolesDB userContext)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _userContext = userContext;
        }

        // GET: LearningOutcomeNotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LearningOutcomeNote.ToListAsync());
        }

        // GET: LearningOutcomeNotes/ById
        [HttpPost]
        public async Task<IActionResult> ById([FromBody] int[] ids)
        {
            if(ids == null)
            {
                return Json(new { success = false });
            }

            List<LearningOutcomeNote> notes = new List<LearningOutcomeNote>();

            foreach(int id in ids)
            {
                LearningOutcomeNote note = await _context.LearningOutcomeNote.Where(m => m.LOId == id).FirstOrDefaultAsync();
                
                    notes.Add(note);
                
            }

            return Json(new { success = true, notes = notes });
        }

        // GET: LearningOutcomeNotes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LearningOutcomeNote learningOutcomeNote)
        {
            if(learningOutcomeNote == null)
            {
                return Json(new { success = false });
            }

            _context.LearningOutcomeNote.Add(learningOutcomeNote);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] LearningOutcomeNote learningOutcomeNote)
        {
            if(learningOutcomeNote == null)
            {
                return Json(new { success = true });
            }

            learningOutcomeNote.modifiedDate = DateTime.UtcNow;
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            bool role = await _userManager.IsInRoleAsync(user, "Chair");
            if (role)
            {
                learningOutcomeNote.lastEditChair = true;
            }
            else
            {
                learningOutcomeNote.lastEditChair = false;
            }
            _context.Update(learningOutcomeNote);
            await _context.SaveChangesAsync();
            return Json(new { success = true });

        }
      



        // GET: LearningOutcomeNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcomeNote = await _context.LearningOutcomeNote
                .FirstOrDefaultAsync(m => m.LearningOutcomeNoteId == id);
            if (learningOutcomeNote == null)
            {
                return NotFound();
            }

            return View(learningOutcomeNote);
        }

        // POST: LearningOutcomeNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningOutcomeNote = await _context.LearningOutcomeNote.FindAsync(id);
            _context.LearningOutcomeNote.Remove(learningOutcomeNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningOutcomeNoteExists(int id)
        {
            return _context.LearningOutcomeNote.Any(e => e.LearningOutcomeNoteId == id);
        }
    }
}
