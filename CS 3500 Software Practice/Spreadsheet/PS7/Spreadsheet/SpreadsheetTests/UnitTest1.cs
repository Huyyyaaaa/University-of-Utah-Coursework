using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using Dependencies;
using SS;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SpreadsheetTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// test spreadsheet constructor
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            
        }
        /// <summary>
        /// test GetCellContents with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod2()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.GetCellContents(null);
        }
        /// <summary>
        /// test GetCellContents with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod3()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.GetCellContents("hello");
        }

        /// <summary>
        /// test getCellContents
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "5");
            s1.GetCellContents("A1");
        }
        /// <summary>
        /// test getallnonemptycells
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "5");
            s1.GetNamesOfAllNonemptyCells();
        }

        /// <summary>
        /// test setcellcontents(name, double) with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod6()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell(null, "5");

        }

        /// <summary>
        /// test setcellcontents(name, double) with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod7()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("hello", "5");

        }
        /// <summary>
        /// test setcellcontents(name, double)with same cell name
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "5");
            s1.SetContentsOfCell("A1", "3");
        }

        /// <summary>
        /// test setcellcontents(name, text) with null name 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod9()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell(null, "test");

        }
        /// <summary>
        /// test setcellcontents(name, text) with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod10()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("hello", "test");

        }

        /// <summary>
        /// test setcellcontents(name, text) with null text
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod11()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", null);

        }

        /// <summary>
        /// test setcellcontents(name, text)
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "test");
        }

        /// <summary>
        /// test setcellcontents(name, text) with already existing name
        /// </summary>
        [TestMethod]
        public void TestMethod13()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "test");
            s1.SetContentsOfCell("A1", "gersf");
        }

        /// <summary>
        /// test setcellcontents(name, text) with empty string text
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod14()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "");

        }

        /// <summary>
        /// test setcellcontents(name, formula) with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod15()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell(null, "=2+3");

        }

        /// <summary>
        /// test setcellcontents(name, formula) with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod16()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("hello", "=2+3");

        }
        /// <summary>
        /// test setcellcontents(name, formula)
        /// </summary>
        [TestMethod]
        public void TestMethod17()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=2+3");
        }

        /// <summary>
        /// test setcellcontents(name, formula) with existing cell
        /// </summary>
        [TestMethod]
        public void TestMethod18()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=2+3");
            s1.SetContentsOfCell("A1", "=4+3");
        }

        /// <summary>
        /// test setcellcontents(name, formula) for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestMethod19()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=A1");
            
            
        }

        [TestMethod]
        public void TestMethod20()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "5");
            s1.SetContentsOfCell("A2", "5");
            s1.SetContentsOfCell("A3", "=A1+A2");
            Assert.AreEqual(10.0, s1.GetCellValue("A3"));
        }

        /// <summary>
        /// Tests for SetCellContents (Formula)
        /// no longer works for PS6, not needed
        /// </summary>
        //[TestMethod]
        //public void TestMethod21()
        //{
        //    AbstractSpreadsheet s1 = new Spreadsheet();
        //    s1.SetContentsOfCell("A1", "=C1+B1");
        //    object value = s1.GetCellContents("A1");
        //    Assert.AreEqual(new Formula("C1 + B1").ToString(), value.ToString());
        //}

        [TestMethod]
        public void TestMethod22()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "50");
            Assert.AreEqual(50.0, s1.GetCellContents("A1"));
        }

        /// <summary>
        /// tests save spreadsheet
        /// </summary>
        [TestMethod]
        public void TestMethod23()
        {
            string path = @"..\..\testfiles\s2.xml";
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "50");
            System.IO.TextWriter writer = File.CreateText(path);
            s1.Save(writer);
        }
        /// <summary>
        /// check read spreadsheet file
        /// </summary>
        [TestMethod]
        public void TestMethod24()
        {
            string path = @"..\..\testfiles\s1.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
            Assert.AreEqual(50.0, s1.GetCellValue("A1"));
        }

        /// <summary>
        /// test spreadsheet constructor with regex parameter
        /// </summary>
        [TestMethod]
        public void TestMethod25()
        {
            string r = "[a-zA-Z][0-9]";
            AbstractSpreadsheet s1 = new Spreadsheet(new System.Text.RegularExpressions.Regex(r));
        }

        /// <summary>
        /// test for inavlid spreadsheet read
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod26()
        {
            string path = @"..\..\testfiles\s3.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        /// <summary>
        /// test for inavlid spreadsheet read
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod27()
        {
            string path = @"..\..\testfiles\s4.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        /// <summary>
        /// test for inavlid spreadsheet read
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod28()
        {
            
            string path = @"..\..\testfiles\s5.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        /// <summary>
        /// test for file doesnt exist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestMethod29()
        {
            string path = @"..\..\testfiles\ss.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        /// <summary>
        /// test GetCellValue for null name parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod30()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "56");
            s1.GetCellValue(null);
        }

        /// <summary>
        /// test GetCellValue for invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod31()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "56");
            s1.GetCellValue("hello");
        }

        /// <summary>
        /// test save spreadsheet with more than one cell
        /// </summary>
        [TestMethod]
        public void TestMethod32()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=5+4");
            s1.SetContentsOfCell("A2", "word");
            string path = @"..\..\testfiles\s6.xml";
            System.IO.TextWriter writer = File.CreateText(path);
            s1.Save(writer);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod33()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell(null, "6");
            s1.SetContentsOfCell("A2", "word");
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestMethod34()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("hgld", "6");
            s1.SetContentsOfCell("A2", "word");

        }

        [TestMethod]
        public void TestMethod35()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A2", "5");
            Assert.AreEqual(true, s1.Changed);

        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod36()
        {
            string path = @"..\..\testfiles\s7.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod37()
        {
            string path = @"..\..\testfiles\s8.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod38()
        {
            string path = @"..\..\testfiles\s9.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestMethod39()
        {
            string path = @"..\..\testfiles\s10.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(path);
        }


        [TestMethod]
        public void StressTest1()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "hello");
            s1.SetContentsOfCell("A2", "55");
            s1.SetContentsOfCell("A3", "=2+2");
            Assert.AreEqual(55.0, s1.GetCellContents("A2"));
            Assert.AreEqual(4.0, s1.GetCellValue("A3"));
        }

        [TestMethod]
        public void StressTest2()
        {
            StressTest1();
        }

        [TestMethod]
        public void StressTest3()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "hello");
            s1.SetContentsOfCell("A2", "55");
            s1.SetContentsOfCell("A3", "=2+2");
            HashSet<string> h = new HashSet<string>();
            h.Add("A1");
            h.Add("A2");
            h.Add("A3");
           
            List<string> hash = new List<string>();
            List<string> cells = new List<string>();

            foreach(string s in s1.GetNamesOfAllNonemptyCells())
            {
                cells.Add(s);
            }
            foreach(string s in h)
            {
                hash.Add(s);
            }

            for(int i = 0; i < cells.Count; i++)
            {
                Assert.AreEqual(cells[i], hash[i]);
            }

        }
       
       
    }
}
