/**
 * Author:    Aaron Templeton
 * Date:      10/31/19
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Aaron Templeton - This work may not be copied for use in Academic Coursework.
 *
 * I, Aaron Templeton, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Web Scraper for U of U CS Courses using Selenium
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using Keys = OpenQA.Selenium.Keys;

namespace WebScraper
{
    public partial class Form1 : Form
    {
        public List<Course> savedCourses;
        public int iterator;
        public Form1()
        {
            InitializeComponent();
            savedCourses = new List<Course>();
            iterator = 0;
        }

        public void beginScrape()
        {
            status1.Text = "";
            status1.ForeColor = System.Drawing.Color.Green;
            status1.Text = "Scraping data...";

            string baseUrl = "https://student.apps.utah.edu/uofu/stu/ClassSchedules/main/";
            string year = yearTextBox.Text;
            string semester = semesterBox.Text;

            string yearUrl = year.Substring(year.Length - 2);
            string semUrl = "";
            switch (semester)
            {
                case "Spring":
                    semUrl = "4";
                    break;
                case "Summer":
                    semUrl = "6";
                    break;
                case "Fall":
                    semUrl = "8";
                    break;
            }

            string fullUrl = baseUrl + "1" + yearUrl + semUrl + "/class_list.html?subject=CS";

            scrapeCourse(fullUrl);
        }
        /// <summary>
        /// Scrapes the course enrollments of the U of U course catalog
        /// </summary>
        /// <param name="fullUrl"></param>
        public void scrapeCourse(string fullUrl) { 

            var _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl(fullUrl);
            IWebElement el = _driver.FindElement(By.LinkText("Seating availability for all CS classes"));
            el.Click();

            var tbl = _driver.FindElement(By.TagName("table"));// get table
            var rows = tbl.FindElements(By.XPath("//tbody//tr"));// get rows in table
 

            int counter = 1;//loop counter
            int amountSaved = 0;//amount of courses saved
            string[] course = courseBox.Text.Split(',');

            if(courseBox.Text == "")
            {
                foreach (var r in rows)
                {
                    string title = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[5]")).Text;
                    if ( !(title.Contains("Seminar")) && !(title.Contains("Special Topics"))) // does not contain seminar or special topics
                    {
                        int classNum = int.Parse(tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[3]")).Text); // get catalog number of course
                        if (classNum > 1000 && classNum < 7000)
                        {
                            string section = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[4]")).Text; // get section 001 only
                            if (section == "001")
                            {
                                
                                string enroll = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[7]")).Text;

                                savedCourses.Add(new Course
                                { Dept = "CS",
                                    Number = classNum,
                                    Semester = semesterBox.Text,
                                    Title = title,
                                    Year = yearTextBox.Text,
                                    Enrollment = enroll,
                                    Description = "",
                                    Credits = ""
                                });
                                amountSaved++;
                            }
                        }
                    }

                    counter++;
                    if (amountSaved == int.Parse(amountBox.Text)) break;
                }
            }
            else
            {
                foreach (var r in rows)
                {
                    string title = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[5]")).Text;
                    if (!(title.Contains("Seminar")) && !(title.Contains("Special Topics"))) // does not contain seminar or special topics
                    {
                        int classNum = int.Parse(tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[3]")).Text); // get catalog number of course
                        if (course.Contains(classNum.ToString()))
                        {
                            string section = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[4]")).Text; // get section 001 only
                            if (section == "001")
                            {
                               
                                string enroll = tbl.FindElement(By.XPath("//tbody//tr[" + counter + "]//td[7]")).Text;

                                savedCourses.Add(new Course
                                {
                                    Dept = "CS",
                                    Number = classNum,
                                    Semester = semesterBox.Text,
                                    Title = title,
                                    Year = yearTextBox.Text,
                                    Enrollment = enroll,
                                    Description = "",
                                    Credits = ""
                                });
                                amountSaved++;
                            }
                        }
                    }

                    counter++;
                    if (amountSaved == int.Parse(amountBox.Text)) break;
                }
            }

            if(amountSaved == 0)
            {
                status1.ForeColor = System.Drawing.Color.Red;
                status1.Text = "Course not found";
                _driver.Close();
                _driver.Dispose();
            }
            else
            {
                //get description for each course
                scrapeDescription(_driver);
            }
            
        }
        /// <summary>
        /// get the description and credits for each course
        /// </summary>
        /// <param name="driver"></param>
        public void scrapeDescription(ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            int i;

            for (i = iterator; i < savedCourses.Count; i++)
            {
                driver.Navigate().GoToUrl("https://catalog.utah.edu");

                try
                {
                    IWebElement btn =  driver.FindElement(By.XPath("//*[@id='top']/div/div[3]/div/nav/ul/li[3]/div")); //courses button
                    btn.Click();
                    IWebElement search = driver.FindElement(By.Id("Search"));
                    search.SendKeys("CS" + savedCourses[i].Number.ToString());
                    search.SendKeys(Keys.Return);
                    IWebElement el = driver.FindElement(By.XPath("//*[@id='__KUALI_TLP']/div/table/tbody/tr/th/a"));
                    el.Click();

                    string credit = driver.FindElement(By.XPath("//*[text()='Credits']/following-sibling::div/div")).Text;
                    if (credit.Contains("-"))
                    {
                        credit = credit.Substring(credit.Length - 1);
                    }
                    
                    string desc = driver.FindElement(By.XPath("//*[text()='Course Description']/following-sibling::div/div")).Text;

                    savedCourses[i].Credits = credit;
                    savedCourses[i].Description = desc;

                    status1.Text = "Data successfully retrieved";
                }
                catch(Exception e)
                {
                    status1.ForeColor = System.Drawing.Color.Red;
                    status1.Text = "error retrieving data. Try again?";
                 
                }
                


            }
            
            iterator = i;
            driver.Close();
            driver.Dispose();
            //reset buttons and inputs
            save1.Enabled = true;
        }
        /// <summary>
        /// exports the savedCourses to a csv file
        /// </summary>
        public void exportToCSV()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Department,Number,Credits,Title,Enrollment,Semester,Year,Description");
            foreach(Course c in savedCourses)
            {
                c.Description = c.Description.Replace(",", "");
                c.Description = c.Description.Replace("\r\n", "");
                c.Description = c.Description.Replace("\n", "");
                c.Description = c.Description.Replace("\r", "");
                sb.AppendLine(c.Dept+","+c.Number+","+c.Credits+","+c.Title+","+c.Enrollment+","+c.Semester+","+c.Year+","+c.Description);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save As";
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                    status1.ForeColor = System.Drawing.Color.Green;
                    status1.Text = "File saved successfully!";
                }
                catch (Exception e)
                {
                    status1.ForeColor = System.Drawing.Color.Red;
                    status1.Text = "Unable to save. Make sure file is not open";
                }
            }

           
             
        }

        //event handlers
        private void submit1_Click(object sender, EventArgs e)
        {
            if(yearTextBox.Text == "" || semesterBox.Text == "" || yearTextBox.Text.Length < 4 || yearTextBox.Text.Length > 4)
            {
                status1.Text = "Year must be 4 characters (e.g. 2020), Semester must be chosen";
            }
            else
            {
                beginScrape();
            }
            
        }

        private void save1_Click(object sender, EventArgs e)
        {
            if(savedCourses.Count > 0)
            {
                exportToCSV();
            }
            else
            {
                status1.Text = "No data has been retrieved";
            }
        }
    }
    /// <summary>
    /// a class object for the Course
    /// </summary>
    public class Course
    {
        public string Dept { get; set; }   
        public int Number { get; set; }
        public string Credits { get; set; }
        public string Title { get; set; }
        public string Enrollment { get; set; }
        public string Semester { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
    }
}
