// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependents; //dictionary for dependents
        private Dictionary<string, HashSet<string>> dependees; //dictionary for dependees
        private int size;//size of Graph
        
        /// <summary>
        /// creates a new dependency graph from an existing dependency graph
        /// </summary>
        /// <param name="dg"></param>
        public DependencyGraph(DependencyGraph dg) :this() {
            foreach(string d in dg.dependees.Keys)
            {
                foreach(string s in dg.GetDependents(d))
                {
                    this.AddDependency(d, s);
                }
            }

                    }

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {//get size
               
                return size;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            //check if s is null
            if (s == null) throw new ArgumentNullException("Null arguments not allowed");
            //when dependents string s exists 
            if (dependees.ContainsKey(s)){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            //check if s is null
            if (s == null) throw new ArgumentNullException("Null arguments not allowed");

            //when dependees string s exists
            if (dependents.ContainsKey(s))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //check if s is null

            if (s == null) throw new ArgumentNullException("Null arguments not allowed");

            //returns a hashset of the dependents of s

            if (dependees.ContainsKey(s))
            {
                return new HashSet<string>(dependees[s]);
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //check if s is null

            if (s == null) throw new ArgumentNullException("Null arguments not allowed");

            //returns a hashshet of the dependees of s
            if (dependents.ContainsKey(s))
            {
                return new HashSet<string>(dependents[s]);
            
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            //dont add if null parameter given
            if(s==null || t == null)
            {
                throw new ArgumentNullException("cannot have null values");
            }

            //if dependency already exists, dont add it again
            if (!(dependees.ContainsKey(s) && dependees[s].Contains(t)))
            {
                size++;
                //if dependee exists, add dependent only   
                if (dependees.ContainsKey(s))
                {
                    dependees[s].Add(t);
                }
                else
                {
                    HashSet<string> x = new HashSet<string>();
                    x.Add(t);
                    dependees.Add(s, x);
                }

                //if dependent exists add dependee only
                if (dependents.ContainsKey(t))
                {
                    dependents[t].Add(s);
                }
                else
                {
                    HashSet<string> x = new HashSet<string>();
                    x.Add(s);
                    dependents.Add(t, x);
                }
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            //check if s or t is null

            if (s == null || t == null)
            {
                throw new ArgumentNullException("cannot have null values");
            }

            if (dependees.ContainsKey(s))
            {
                size--;
            }
            //removes dependent of s
            if (dependees.ContainsKey(s))
            {
                dependees[s].Remove(t);
                
                //if dependent is last dependent, remove dependee too
                if (dependees[s].Count == 0)
                {
                    dependees.Remove(s);
                }
            }

            //removes dependee of t
            if (dependents.ContainsKey(t))
            {
                dependents[t].Remove(s);
                //if dependee is last, remove dependent too
                if (dependents[t].Count == 0)
                {
                    dependents.Remove(t);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //check if s or newDependents is null

            if (s == null || newDependents == null)
            {
                throw new ArgumentNullException("cannot have null values");
            }

            //current dependents
            IEnumerable<string> current = GetDependents(s);

            //remove all current dependents s,r
            foreach (string r in current)
            {
                
                RemoveDependency(s, r);
            }

            //add new dependency s,t
            foreach(string t in newDependents)
            {
                //check that t is not null
                if (t == null) throw new ArgumentNullException("cannot have null values in newDependents");
                AddDependency(s, t);
            }
        }
        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            //check that t and new dependees not null
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException("cannot have null values");
            }

            //get current dependees
            IEnumerable<string> current = GetDependees(t);

            //remove dependencies r,t
            foreach(string r in current)
            {
                RemoveDependency(r, t);
            }

            //add dependecies s,t
            foreach(string s in newDependees)
            {
                //make sure s is not null
                if (s == null) throw new ArgumentNullException("cannot have null values in newDependees");
                AddDependency(s, t);
            }
        }
    }
}
