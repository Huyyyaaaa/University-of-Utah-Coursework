// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Text.RegularExpressions;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x2 + y2");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x1 + y1");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x1 + y1) * (z1 / x1) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x1": return 4.0;
                case "y1": return 6.0;
                case "z1": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void PS4Test1()
            {
                Formula f = new Formula("x2", x => x.ToUpper(), x => true);
                                
                    Assert.AreEqual("X2", f.ToString());
                
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test2()
            {
                Formula f = new Formula("xx2", x => x.ToUpper(), x => false);
                Assert.Fail();
            }

            [TestMethod]
            public void PS4Test3()
            {
                Formula f = new Formula("x2+y3", x => x.ToUpper(), x => true);

                  Assert.AreEqual("X2+Y3", f.ToString());
                

            }


            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test4()
            {
                Formula f = new Formula("x2 + y", x => x.ToUpper(), x => Regex.IsMatch(x, @"^[A-Z]{1}[0-9]{1}$"));
                Assert.Fail();
            }


            [TestMethod]
            public void PS4Test5()
            {
                Formula f = new Formula("x2+y3", x => x.ToUpper(), x => true);
                Assert.AreEqual("X2+Y3", f.ToString());
            }


            [TestMethod]
            public void PS4Test6()
            {
                Formula f = new Formula("x2 + y3");
                Assert.AreEqual(10, f.Evaluate(x => 5));

            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void PS4Test7()
            {
                Formula f = new Formula("x2 + y3", null, x => true);
                Assert.Fail();

            }

            //[TestMethod]
            //[ExpectedException(typeof(ArgumentNullException))]
            //public void PS4Test8()
            //{
            //    Formula f = new Formula(null);
            //    Assert.Fail();

            //}

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test9()
            {
                Formula f = new Formula("");
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test10()
            {
                Formula f = new Formula("4+");
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test11()
            {
                Formula f = new Formula("(4))");
            }

            [TestMethod]

            public void PS4Test12()
            {
                Formula f = new Formula("x4 + f3 + r4", x => x.ToUpper(), x => true);
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test13()
            {
                Formula f = new Formula("(((4))");
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void PS4Test14()
            {
                Formula f = new Formula("4 + 4");
                f.Evaluate(null);
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaEvaluationException))]
            public void PS4Test15()
            {
                Formula f = new Formula("4 / 0");
                f.Evaluate(x => 1);
            }

            [TestMethod]

            public void PS4Test16()
            {
                Formula f = new Formula("4 / 1");
                f.Evaluate(x => 1);
            }

            [TestMethod]
            public void PS4Test17()
            {
                Formula f = new Formula("x5 + f4 - 8 * 2");
                f.Evaluate(x => 1);
            }

            [TestMethod]
            public void PS4Test18()
            {
                Formula f = new Formula("x5 + f4 - 8 * 2", x => x.ToUpper(), x => true);
                f.GetVariables();
            }

            [TestMethod]
            [ExpectedException(typeof(FormulaFormatException))]
            public void PS4Test19()
            {
                Formula f = new Formula("  ");
            }

            [TestMethod]
            public void PS4Test20()
            {
                Formula f = new Formula("(8+4)/2*4+(8-2)");
                f.Evaluate(x => 1);
            }

            [TestMethod]
            public void PS4Test21()
            {
                Formula f = new Formula();
                Assert.AreEqual(0, f.Evaluate(x => 0));
            }

        }
    }
}
