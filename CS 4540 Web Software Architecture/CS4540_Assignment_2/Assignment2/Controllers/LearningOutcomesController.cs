/**
 * Author:    Aaron Templeton
 * Date:      9/27/19
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Aaron Templeton - This work may not be copied for use in Academic Coursework.
 *
 * I, Aaron Templeton, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Learning OUtcomes Controller for CRUD operations on learing outcomes
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Assignment2.Controllers
{
    public class LearningOutcomesController : Controller
    {
        private readonly LOContext _context;

        public LearningOutcomesController(LOContext context)
        {
            _context = context;
        }

        // GET: LearningOutcomes
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.LearningOutcomes.ToListAsync());
        //}

        // GET: LearningOutcomes/ByCourse/3500
        
        public async Task<IActionResult> ByCourse(int? number)
        {
            if(number == null)
            {
                return NotFound();
            }

            var course = from c in _context.Courses
                    where c.number == number
                    select c;

            ViewData["cid"] = course.First().CourseId;
            ViewData["cname"] = course.First().name;

            var learningOutcome = await _context.LearningOutcomes
                .Where(m => m.CourseNum == number).ToListAsync<LearningOutcome>();

            if(learningOutcome == null)
            {
                return NotFound();
            }

            return View("ByCourse", learningOutcome);
        }

        // GET: LearningOutcomes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var learningOutcome = await _context.LearningOutcomes
        //        .FirstOrDefaultAsync(m => m.LearningOutcomeId == id);
        //    if (learningOutcome == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(learningOutcome);
        //}

        // GET: LearningOutcomes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearningOutcomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearningOutcomeId,name,description,CourseNum")] LearningOutcome learningOutcome)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learningOutcome);
                await _context.SaveChangesAsync();
                return RedirectToAction("ByCourse", new { number = learningOutcome.CourseNum});
            }
            return View(learningOutcome);
        }

        // GET: LearningOutcomes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LearningOutcomes.FindAsync(id);
            if (learningOutcome == null)
            {
                return NotFound();
            }
            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearningOutcomeId,name,description,CourseNum")] LearningOutcome learningOutcome)
        {
            //if (id != learningOutcome.LearningOutcomeId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningOutcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningOutcomeExists(learningOutcome.LearningOutcomeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ByCourse", new { number = learningOutcome.CourseNum });
            }
            return View(learningOutcome);
        }

        // GET: LearningOutcomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcome = await _context.LearningOutcomes
                .FirstOrDefaultAsync(m => m.LearningOutcomeId == id);
            if (learningOutcome == null)
            {
                return NotFound();
            }

            return View(learningOutcome);
        }

        // POST: LearningOutcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningOutcome = await _context.LearningOutcomes.FindAsync(id);
            _context.LearningOutcomes.Remove(learningOutcome);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        private bool LearningOutcomeExists(int id)
        {
            return _context.LearningOutcomes.Any(e => e.LearningOutcomeId == id);
        }
    }
}
