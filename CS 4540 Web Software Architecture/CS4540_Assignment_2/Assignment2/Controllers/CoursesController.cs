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
 *    Courses Controller for CRUD operations on courses
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;


namespace Assignment2.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LOContext _context;

        public CoursesController(LOContext context)
        {
            _context = context;
        }

        // GET: Courses
        [Authorize(Roles= "Admin, Instructor")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Instructor"))
            {
                var courses = await _context.Courses
                    .Where(m => m.professorEmail == User.Identity.Name)
                    .ToListAsync();
                return View(courses);
            }
            else
            {
                return View(await _context.Courses.ToListAsync());
            }
            
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Admin, Instructor, Chair")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if(User.IsInRole("Instructor"))
            {
                var c = await _context.Courses
                    .Where(m => m.professorEmail == User.Identity.Name)
                    .FirstOrDefaultAsync(m => m.CourseId == id);

                if(c == null)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                return View(c);
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }


            return View(course);
            
        }


        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,name,description,department,number,semeseter,year, professor, professorEmail")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,name,description,department,number,semeseter,year, professor, professorEmail")] Course course)
        {
            //if (id != course.CourseId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = course.CourseId });
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
