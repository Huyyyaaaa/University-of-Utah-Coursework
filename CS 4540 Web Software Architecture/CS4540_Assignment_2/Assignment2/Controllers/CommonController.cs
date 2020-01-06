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
 *    Common Controller for miscellaneous queries and CRUD operations
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    public class CommonController : Controller
    {
        private readonly LOContext _context;

        public CommonController(LOContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Chair")]
        public IActionResult Chair()
        {
            var courses = from c in _context.Courses select c;
            var los = from l in _context.LearningOutcomes select l;

            dynamic model = new ExpandoObject();
            model.Courses = courses;
            model.Los = los;
            return View(model);
        }
    }
}