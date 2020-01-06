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
 *    User Permissions Controller for CRUD operations on users and their roles
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assignment2.Controllers
{
    public class UsersPermissionsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly UserRolesDB _context;
        public UsersPermissionsController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, UserRolesDB context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {

            //var users = _context.Users.ToList();
            //var userRoles = new Dictionary<string, List<string>>();
            //foreach(IdentityUser u in users)
            //{
            //    List<string> role = await getUserRole(u);
            //    userRoles.Add(u.UserName, role);
            //}

            //dynamic model = new ExpandoObject();
            //model.users =  userRoles;

            //return View(model);
            return View();
        }

        public async Task<List<string>> getUserRole(IdentityUser user)
        {
            List<string> role = new List<string>();

            bool admin = await _userManager.IsInRoleAsync(user, "Admin");
            bool chair = await _userManager.IsInRoleAsync(user, "Chair");
            bool inst = await _userManager.IsInRoleAsync(user, "Instructor");
            if (admin)
            {
                role.Add("Admin");
            }
             if(chair)
            {
                role.Add("Chair");
            }
            if(inst)
            {
                role.Add("Instructor");
            }

            return role;
        }
        [HttpPost]
        public async Task<IActionResult> changeRole([FromBody] Role role)
        {
            IdentityUser user = await _userManager.FindByNameAsync(role.User);
            string changedRole = "";
            switch (role.Num)
            {
                case 0:
                    changedRole = "Admin";
                    break;
                case 1:
                    changedRole = "Chair";
                    break;
                case 2:
                    changedRole = "Instructor";
                    break;
            }

            bool inRole = await _userManager.IsInRoleAsync(user, changedRole);

               if (changedRole == "Admin")
                {
                int admins = await numberOfAdmins();
                if (inRole && admins > 1)
                    {
                        await _userManager.RemoveFromRoleAsync(user, changedRole);
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "To remove an Admin there must be 2 or more Admins" });
                    }
                }
                else if (inRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, changedRole);
                    return Json(new { success = true });
                }
            else
            {
                await _userManager.AddToRoleAsync(user, changedRole);
                return Json(new { success = true });
            }


        }
            

        public async Task<int> numberOfAdmins()
        {
            int number = 0;
            var users = _context.Users.ToList();
            foreach(IdentityUser u in users)
            {
                var x = await _userManager.IsInRoleAsync(u, "Admin");
                if (x) number++;

            }
            return number;
        }

        public async Task<IActionResult> getUsersRoles()
        {
            List<User> users = new List<User>();

            var us = _context.Users.ToList();
            foreach (IdentityUser u in us)
            {
                List<string> role = await getUserRole(u);
                users.Add(new User(u.UserName, role));
            }

            return Json(new { success = true, data = users });
        }
    }

    public class Role
    {
        public string User { get; set; }
        public int Num { get; set; }
    };

    public class User
    {
       public User(string name, List<string> roles)
        {
            Name = name;
            Roles = roles;
        }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    };
}