using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
  [Authorize(Roles = "Professor")]
  public class ProfessorController : CommonController
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Students(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Class(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Categories(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      return View();
    }

    public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }

    public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      ViewData["uid"] = uid;
      return View();
    }

    /*******Begin code to modify********/


    /// <summary>
    /// Returns a JSON array of all the students in a class.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "dob" - date of birth
    /// "grade" - the student's grade in this class
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
    {
      var query =
        from co in db.Courses
        join c in db.Classes
        on co.CatalogId equals c.CatalogId
        join e in db.Enrolled
        on c.ClassId equals e.ClassId
        join s in db.Students
        on e.UId equals s.UId
        where subject == co.DepartmentNavigation.Subject
        where num == co.Num
        where season == c.Semester.Substring(0, c.Semester.IndexOf(" "))
        where year == int.Parse(c.Semester.Substring(c.Semester.IndexOf(" ") + 1))
        select new
        {
          fname = s.FName,
          lname = s.LName,
          uid = s.UId,
          dob = s.Dob,
          grade = e.Grade
        };

      return Json(query.ToArray());
    }



    /// <summary>
    /// Returns a JSON array with all the assignments in an assignment category for a class.
    /// If the "category" parameter is null, return all assignments in the class.
    /// Each object in the array should have the following fields:
    /// "aname" - The assignment name
    /// "cname" - The assignment category name.
    /// "due" - The due DateTime
    /// "submissions" - The number of submissions to the assignment
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class, 
    /// or null to return assignments from all categories</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
    {

      if (category == null)
      {
        var query =
          from co in db.Courses
          join c in db.Classes
          on co.CatalogId equals c.CatalogId
          join ac in db.AssignmentCategories
          on c.ClassId equals ac.ClassId
          join a in db.Assignments
          on ac.CatId equals a.CatId
          where subject == co.DepartmentNavigation.Subject
          where num == co.Num
          where season == c.Semester.Substring(0, c.Semester.IndexOf(" "))
          where year == int.Parse(c.Semester.Substring(c.Semester.IndexOf(" ") + 1))
          select new
          {
            aname = a.Name,
            cname = ac.Name,
            due = a.Due,
            submissions = a.Submissions.Count
          };

        return Json(query.ToArray());
      }
      else
      {

        var query =
          from co in db.Courses
          join c in db.Classes
          on co.CatalogId equals c.CatalogId
          join ac in db.AssignmentCategories
          on c.ClassId equals ac.ClassId
          join a in db.Assignments
          on ac.CatId equals a.CatId
          where subject == co.DepartmentNavigation.Subject
          where num == co.Num
          where season == c.Semester.Substring(0, c.Semester.IndexOf(" "))
          where year == int.Parse(c.Semester.Substring(c.Semester.IndexOf(" ") + 1))
          where category == ac.Name
          select new
          {
            aname = a.Name,
            cname = ac.Name,
            due = a.Due,
            submissions = a.Submissions.Count
          };

        return Json(query.ToArray());


      }
    }


    /// <summary>
    /// Returns a JSON array of the assignment categories for a certain class.
    /// Each object in the array should have the folling fields:
    /// "name" - The category name
    /// "weight" - The category weight
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
    {
      var query =
        from co in db.Courses
        join c in db.Classes
        on co.CatalogId equals c.CatalogId
        join ac in db.AssignmentCategories
        on c.ClassId equals ac.ClassId
        where subject == co.DepartmentNavigation.Subject
        where num == co.Num
        where season == c.Semester.Substring(0, c.Semester.IndexOf(" "))
        where year == int.Parse(c.Semester.Substring(c.Semester.IndexOf(" ") + 1))
        select new
        {
          name = ac.Name,
          weight = ac.Weight
        };

      return Json(query.ToArray());
    }

    /// <summary>
    /// Creates a new assignment category for the specified class.
    /// If a category of the given class with the given name already exists, return success = false.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The new category name</param>
    /// <param name="catweight">The new category weight</param>
    /// <returns>A JSON object containing {success = true/false} </returns>
    public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
    {
      try
      {
        int classID = 0;

        var query =
          from cl in db.Classes
          join co in db.Courses
          on cl.CatalogId equals co.CatalogId
          where co.DepartmentNavigation.Subject == subject
          where cl.Semester == season + " " + year
          select new
          {
            id = cl.ClassId
          };

        foreach (var q in query)
        {
          classID = (int)q.id;
        }

        AssignmentCategories c = new AssignmentCategories();
        c.Name = category;
        c.Weight = (byte?)catweight;
        c.ClassId = (uint)classID;
        db.AssignmentCategories.Add(c);
        db.SaveChanges();
        return Json(new { success = true });

      }
      catch (Exception e)
      {
        return Json(new { success = false });
      }
    }

    /// <summary>
    /// Creates a new assignment for the given class and category.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The new assignment name</param>
    /// <param name="asgpoints">The max point value for the new assignment</param>
    /// <param name="asgdue">The due DateTime for the new assignment</param>
    /// <param name="asgcontents">The contents of the new assignment</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
    {
      try
      {
        uint catid = 0;
        int classid = 0;

        var query =
          from c in db.Classes
          join co in db.Courses
          on c.CatalogId equals co.CatalogId
          where co.DepartmentNavigation.Subject == subject
          where co.Num == num
          where c.Semester == season + " " + year
          select new
          {
            cid = c.ClassId
          };

        foreach (var q in query)
        {
          classid = (int)q.cid;
        }

        var query2 =
          from ac in db.AssignmentCategories
          where ac.Name == category
          where ac.ClassId == classid
          select new
          {
            cd = ac.CatId
          };

        foreach (var q in query2)
        {
          catid = q.cd;
        }

        Assignments a = new Assignments();
        a.Name = asgname;
        a.Contents = asgcontents;
        a.Due = asgdue;
        a.Points = (uint?)asgpoints;
        a.CatId = catid;
        db.Assignments.Add(a);
        db.SaveChanges();

        //update all students grade in class
        UpdateClassGrades(classid);

        return Json(new { success = true });
      }
      catch (Exception e)
      {
        return Json(new { success = false });
      }
    }

    /// <summary>
    /// Updates all students grades in a given class
    /// </summary>
    /// <param name="classid">Class ID in which to update all student grades</param>
    private void UpdateClassGrades(int classid)
    {
      var query =
        from e in db.Enrolled
        where e.ClassId == classid
        select new
        {
          uid = e.UId
        };

      foreach (var q in query)
      {
        UpdateStudentGrade(q.uid, classid);
      }
    }

    /// <summary>
    /// Gets a JSON array of all the submissions to a certain assignment.
    /// Each object in the array should have the following fields:
    /// "fname" - first name
    /// "lname" - last name
    /// "uid" - user ID
    /// "time" - DateTime of the submission
    /// "score" - The score given to the submission
    /// 
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
    {
      var query =
        from a in db.Assignments
        join ac in db.AssignmentCategories
        on a.CatId equals ac.CatId
        join c in db.Classes
        on ac.ClassId equals c.ClassId
        join co in db.Courses
        on c.CatalogId equals co.CatalogId
        join s in db.Submissions
        on a.AId equals s.AId
        join st in db.Students
        on s.UId equals st.UId
        where co.DepartmentNavigation.Subject == subject
        where co.Num == num
        where c.Semester == season + " " + year
        where ac.Name == category
        where a.Name == asgname
        select new
        {
          fname = st.FName,
          lname = st.LName,
          uid = st.UId,
          time = s.Time,
          score = s.Score
        };

      return Json(query.ToArray());
    }


    /// <summary>
    /// Set the score of an assignment submission
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment</param>
    /// <param name="uid">The uid of the student who's submission is being graded</param>
    /// <param name="score">The new score for the submission</param>
    /// <returns>A JSON object containing success = true/false</returns>
    public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
    {
      try
      {
        int classID = 0;

        var classQuery =
          from c in db.Classes
          join co in db.Courses
          on c.CatalogId equals co.CatalogId
          where c.Semester == season + " " + year
          where co.DepartmentNavigation.Subject == subject
          where co.Num == num
          select new
          {
            classID = c.ClassId
          };

        foreach (var q in classQuery)
        {
          classID = (int)q.classID;
        }

        var query =
          from ac in db.AssignmentCategories
          join a in db.Assignments
          on ac.CatId equals a.CatId
          join s in db.Submissions
          on a.AId equals s.AId
          where ac.ClassId == classID
          where ac.Name == category
          where a.Name == asgname
          where s.UId == uid
          select s;

        foreach (var sub in query)
        {
          sub.Score = (uint?)score;
        }

        db.SaveChanges();
        UpdateStudentGrade(uid, classID);
        return Json(new { success = true });
      }
      catch (Exception e)
      {
        return Json(new { success = false });
      }
    }

    /// <summary>
    /// Updates the grade in Enrolled for student in class
    /// </summary>
    /// <param name="uid">The uID of the student getting the new grade</param>
    /// <param name="classID">The classID of the class the student is getting the new grade</param>
    private void UpdateStudentGrade(string uid, int classID)
    {
      var assignmentCatQuery =
        from ac in db.AssignmentCategories
        join c in db.Classes
        on ac.ClassId equals c.ClassId
        where c.ClassId == classID
        select new
        {
          cname = ac.Name,
          weight = ac.Weight,
          assignments =
            from a in db.Assignments
            join s in db.Submissions
            on a.AId equals s.AId
            where a.CatId == ac.CatId
            where s.UId == uid
            select new
            {
              points = a.Points,
              score = s.Score,
              submissions = s,
              a.Submissions // This contains all submissions, even those not matching uID
            }
        };

      var grade_percent = 0.0;
      var weight_total = 0;
      foreach (var cat in assignmentCatQuery)
      {
        var total = (uint?)0;
        var earned = (uint?)0;
        var i = 0;

        foreach (var assignment in cat.assignments)
        {
          if (i == 0) weight_total += (int)cat.weight; // only include weights that have assignments

          total += assignment.points;
          // TODO: This condition and body feels dirty and chance for errors if multiple submissions from same user on same assignment
          if (assignment.submissions.Contents.Count() > 0)
          {
            earned += (assignment.score != null) ? assignment.score : 0;
            //if (assignment.Submissions.Last().Score != null) // if null, earned is not increased
            //  earned += assignment.Submissions.Last().Score;
          }
          i++;
        }
        if (total != 0 && earned != 0)
          grade_percent += (double)(((double)earned / total) * cat.weight);
      }

      double scale = (weight_total == 0) ? 1 : 100.0 / weight_total; // If no weights, weight everything evenly

      grade_percent = grade_percent * scale;

      var letter_grade = "";
      if (grade_percent >= 93)
        letter_grade = "A";
      else if (grade_percent >= 90)
        letter_grade = "A-";
      else if (grade_percent >= 87)
        letter_grade = "B+";
      else if (grade_percent >= 83)
        letter_grade = "B";
      else if (grade_percent >= 80)
        letter_grade = "B-";
      else if (grade_percent >= 77)
        letter_grade = "C+";
      else if (grade_percent >= 73)
        letter_grade = "C";
      else if (grade_percent >= 70)
        letter_grade = "C-";
      else if (grade_percent >= 67)
        letter_grade = "D+";
      else if (grade_percent >= 63)
        letter_grade = "D";
      else if (grade_percent >= 60)
        letter_grade = "D-";
      else
        letter_grade = "E";

      var updateGradeQuery =
        from e in db.Enrolled
        where e.UId == uid
        where e.ClassId == classID
        select e;

      foreach (var grade in updateGradeQuery)
      {
        grade.Grade = letter_grade;
      }

      db.SaveChanges();
    }


    /// <summary>
    /// Returns a JSON array of the classes taught by the specified professor
    /// Each object in the array should have the following fields:
    /// "subject" - The subject abbreviation of the class (such as "CS")
    /// "number" - The course number (such as 5530)
    /// "name" - The course name
    /// "season" - The season part of the semester in which the class is taught
    /// "year" - The year part of the semester in which the class is taught
    /// </summary>
    /// <param name="uid">The professor's uid</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetMyClasses(string uid)
    {
      var query =
        from c in db.Classes
        join co in db.Courses
        on c.CatalogId equals co.CatalogId
        where c.Professor == uid
        select new
        {
          subject = co.DepartmentNavigation.Subject,
          number = co.Num,
          name = co.Name,
          season = c.Semester.Substring(0, c.Semester.IndexOf(" ")),
          year = int.Parse(c.Semester.Substring(c.Semester.IndexOf(" ") + 1))
        };

      return Json(query.ToArray());
    }


    /*******End code to modify********/

  }
}