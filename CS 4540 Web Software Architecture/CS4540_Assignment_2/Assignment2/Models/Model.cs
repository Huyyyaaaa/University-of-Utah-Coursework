using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Assignment2.Models;

namespace Assignment2.Models
{

    public class LOContext: DbContext
    {
        public LOContext(DbContextOptions<LOContext> options) : base(options) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LearningOutcome> LearningOutcomes { get; set; }
        public DbSet<Assignment2.Models.CourseNote> CourseNote { get; set; }
        public DbSet<Assignment2.Models.LearningOutcomeNote> LearningOutcomeNote { get; set; }
    }
    public class Course
    {
        public int CourseId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public int number { get; set; }
        public string semeseter { get; set; }
        public int year { get; set; }
        public string professor { get; set; }   
        public string professorEmail { get; set; }
        public int metricPercent { get; set; }
        public List<LearningOutcome> LearningOutcomes { get; set; }
    }

    public class LearningOutcome
    {
        public int LearningOutcomeId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //Foreign Key for Course
        [Display(Name = "Course")]
        public int CourseNum { get; set; }
        [ForeignKey("number")]
        public virtual Course Course { get; set; }

    }

    public class CourseNote
    {
        public int CourseNoteId { get; set; }
        public string note { get; set; }
        public string status { get; set; }  
        public DateTime modifiedDate { get; set; }
        //Foreign Key for Course
        [Display(Name = "Course")]
        public int CourseNum { get; set; }
        [ForeignKey("number")]
        public virtual Course Course { get; set; }

        public CourseNote()
        {
            this.modifiedDate = DateTime.UtcNow;
        }
    }

    public class LearningOutcomeNote
    {
        public int LearningOutcomeNoteId { get; set; }  
        public string note { get; set; }
        public bool lastEditChair { get; set; }
        public DateTime modifiedDate { get; set; }
        //Foreign Key for Course
        [Display(Name = "LearningOutcome")]
        public int LOId { get; set; }
        [ForeignKey("LearningOutcomeId")]
        public virtual LearningOutcome LearningOutcome { get; set; }

        public LearningOutcomeNote()
        {
            this.modifiedDate = DateTime.UtcNow;
        }
    }
}
