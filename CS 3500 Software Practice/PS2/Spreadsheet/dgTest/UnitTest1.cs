using System;
using System.Collections.Generic;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// this test method tests an empty dependency graph
        /// with correct size of zero
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph d = new DependencyGraph();
            Assert.AreEqual(0, d.Size);
        }
        /// <summary>
        /// tests that empty graph contains no dependencies
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependees("a"));
            Assert.IsFalse(g.HasDependents("a"));
        }

        /// <summary>
        /// tests adding a dependecy to an empty graph
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
        }
        /// <summary>
        /// adding to a graph with existing dependency
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "d");
        }

        /// <summary>
        /// adding duplicate dependency
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("a", "b");
            Assert.AreEqual(1, dg.Size);
        }

        /// <summary>
        /// tests null parameters
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod6()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a",null);
        }

        /// <summary>
        /// test remove dependency
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.RemoveDependency("a", "b");
        }

        /// <summary>
        /// test has dependees and has dependents string s
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.HasDependees("a");
            dg.HasDependents("b");
        }

        /// <summary>
        /// tests get dependees of string s
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "d");
            dg.AddDependency("e", "f");
            dg.GetDependees("b");
        }

        /// <summary>
        /// test get dependents of string s
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "d");
            dg.AddDependency("e", "f");
            dg.GetDependents("a");
        }

        /// <summary>
        /// tests the replace dependees
        /// </summary>
        [TestMethod]
        public void TestMethod11()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> t = new HashSet<string>();
            t.Add("z");
                       
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "d");
            dg.AddDependency("e", "f");
            dg.AddDependency("v", "b");
            dg.AddDependency("a", "n");
            dg.ReplaceDependees("b", t);
            
        }

        /// <summary>
        /// tests replace dependents
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> t = new HashSet<string>();
            t.Add("r");

            dg.AddDependency("a", "b");
            dg.AddDependency("c", "d");
            dg.AddDependency("e", "f");
            dg.AddDependency("a", "w");
            dg.ReplaceDependents("a", t);

        }
        /// <summary>
        /// tests for s == null on add dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod13()
        {
            DependencyGraph dg = new DependencyGraph();
       
            dg.AddDependency(null, "b");


        }

        /// <summary>
        /// tests add if dependee already exists
        /// </summary>
        [TestMethod]
        public void TestMethod14()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("a", "c");

        }

        /// <summary>
        /// tests add if depdendent already exists
        /// </summary>
        [TestMethod]
        public void TestMethod15()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "b");

        }
        /// <summary>
        /// test get dependents on string s that is not in graph
        /// </summary>
        [TestMethod]
        public void TestMethod16()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.GetDependents("r");
        }

        /// <summary>
        /// test get dependees on string s that is not in graph
        /// </summary>
        [TestMethod]
        public void TestMethod17()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.GetDependees("r");
        }

        [TestMethod]
        public void TestMethod18()
        {
            DependencyGraph dg = new DependencyGraph();
            DependencyGraph dg2 = new DependencyGraph(dg);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod19()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.HasDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod20()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.HasDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod21()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.GetDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod22()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.GetDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod23()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.RemoveDependency(null, "b");
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod24()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.RemoveDependency("a", null);

        }

        [TestMethod]
        
        public void TestMethod25()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.HasDependents("a");
        }

        [TestMethod]

        public void TestMethod26()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.HasDependees("b");
        }

        [TestMethod]
        public void TestMethod27()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.RemoveDependency("a", "b");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod28()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            HashSet<string> s = new HashSet<string>();
            s.Add("b");
            dg.ReplaceDependees(null, s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod29()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            HashSet<string> s = new HashSet<string>();
            s.Add(null);
            dg.ReplaceDependees("a", s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod30()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            HashSet<string> s = new HashSet<string>();
            s.Add("a");
            dg.ReplaceDependents(null, s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod31()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            HashSet<string> s = new HashSet<string>();
            s.Add(null);
            dg.ReplaceDependents("a", s);
        }

        [TestMethod]
        public void TestMethod32()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("s", "b");
            dg.RemoveDependency("a", "b");
            dg.RemoveDependency("s", "b");
        }

    }
}
