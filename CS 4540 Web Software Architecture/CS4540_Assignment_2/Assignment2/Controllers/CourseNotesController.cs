using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    public class CourseNotesController : Controller
    {
        private readonly LOContext _context;

        public CourseNotesController(LOContext context)
        {
            _context = context;
        }

        // GET: CourseNotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CourseNote.ToListAsync());
        }

        // GET: CourseNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseNote = await _context.CourseNote
                .FirstOrDefaultAsync(m => m.CourseNoteId == id);
            if (courseNote == null)
            {
                return NotFound();
            }

            return View(courseNote);
        }
        //GET: CourseNotes/ByCourse/5
        public async Task<IActionResult> ByCourse( int? id)
        {
            var courseNote = await _context.CourseNote
                .Where(m => m.CourseNum == id)
                .FirstOrDefaultAsync();
            if(courseNote == null)
            {
                return Json(new { success = true, note = "" });
            }

            return Json(new { success = true, note = courseNote });
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseNote courseNote)
        {
            if (courseNote == null)
            {
                return Json(new { success = false });
            }

            _context.CourseNote.Add(courseNote);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

 
        // POST: CourseNotes/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CourseNote courseNote)
        {
            if(courseNote == null)
            {
                return Json(new { success = false });
            }

            courseNote.modifiedDate = DateTime.UtcNow;
            _context.Update(courseNote);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        public async Task<IActionResult> Approve([FromBody] CourseNote courseNote)
        {
            if (courseNote == null)
            {
                return Json(new { success = false });
            }

            courseNote.modifiedDate = DateTime.UtcNow;
            courseNote.status = "Approved";
            _context.Update(courseNote);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // GET: CourseNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseNote = await _context.CourseNote
                .FirstOrDefaultAsync(m => m.CourseNoteId == id);
            if (courseNote == null)
            {
                return NotFound();
            }

            return View(courseNote);
        }

        // POST: CourseNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseNote = await _context.CourseNote.FindAsync(id);
            _context.CourseNote.Remove(courseNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseNoteExists(int id)
        {
            return _context.CourseNote.Any(e => e.CourseNoteId == id);
        }
    }
}
