/// <summary> 
/// Author:    Isaac Stagg, CS 3500 (a few starting tests)
/// Partner:   None 
/// Date:      1/25/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and [Your Name(s)] - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// This is a unit testing file for the library DependencyGraph. 26 tests cover 100% of the library in its
/// current state.
/// </summary>

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace DevelopmentTests
{
    /// <summary>
    /// This is a test class for DependencyGraphTest and is intended
    /// to contain all DependencyGraphTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        /// Empty graph should contain nothing
        /// </summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Empty graph should contain nothing. Adding and removing
        /// </summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Removing from empty graph should do nothing
        /// </summary>
        [TestMethod()]
        public void RemoveFromEmpty()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Removing something that isn't in the graph shouldn't do anything
        /// </summary>
        [TestMethod()]
        public void RemoveWrongValues()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Adding duplicates should do nothing
        /// </summary>
        [TestMethod()]
        public void AddingDuplicates()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Empty graph should contain nothing, enumerator
        /// </summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Replace on an empty DG shouldn't fail
        /// </summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }

        /// <summary>
        /// It should be possible to have more than one DG at a time.
        /// </summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }

        /// <summary>
        /// Non-empty graph contains something
        /// </summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        /// Testing size using indexer
        /// </summary>
        [TestMethod()]
        public void SizeTestIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(2, t["b"]);
        }

        /// <summary>
        /// Testing hasDependents
        /// </summary>
        [TestMethod()]
        public void HasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependents("a"));
        }

        /// <summary>
        /// Testing hasDependees
        /// </summary>
        [TestMethod()]
        public void HasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependees("b"));
        }

        /// <summary>
        /// Removing a dependency but the dependent has multiple dependees
        /// </summary>
        [TestMethod()]
        public void RemovingDependencyMultipleDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.RemoveDependency("a", "b");
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("c", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Removing a dependency but the dependee has multiple dependents
        /// </summary>
        [TestMethod()]
        public void RemovingDependencyMultipleDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("z", "a");
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.RemoveDependency("z", "a");
            IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            string s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            string s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));
        }

        /// <summary>
        /// Removing multiple at a time should work
        /// </summary>
        [TestMethod()]
        public void BiggerRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.RemoveDependency("a", "c");
            t.RemoveDependency("b", "d");
            Assert.AreEqual(2, t.Size);
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            string s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            string s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
        }

        /// <summary>
        /// Non-empty graph contains something
        /// </summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Non-empty graph contains something
        /// </summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Using lots of data
        /// </summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        /// 3 node graph, replacing one node's dependents with nothing should keep the lower dependency
        /// </summary>
        [TestMethod()]
        public void ReplacingWithEmptyAndNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.ReplaceDependents("a", new List<string>());
            IEnumerator<string> e = t.GetDependents("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("c", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Replacing the middle node's dependents with nothing in a 4 node graph should split it in half.
        /// </summary>
        [TestMethod()]
        public void Replacing4NodesSplitListDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.ReplaceDependents("b", new List<string>());
            IEnumerator<string> e = t.GetDependents("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("d", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// 3 node graph, replacing one node's dependents with nothing should keep the lower dependency
        /// </summary>
        [TestMethod()]
        public void ReplacingWithEmptyAndNoDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.ReplaceDependees("c", new List<string>());
            IEnumerator<string> e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependents("b").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Replacing the middle node's dependees with nothing in a 4 node graph should split it in half.
        /// </summary>
        [TestMethod()]
        public void Replacing4NodesSplitListDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.ReplaceDependees("c", new List<string>());
            IEnumerator<string> e = t.GetDependents("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("d", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Adding empty strings shouldn't do anything
        /// </summary>
        [TestMethod()]
        public void AddingEmptyStrings()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("", "");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Removing empty strings shouldn't do anything
        /// </summary>
        [TestMethod()]
        public void RemovingEmptyStrings()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.RemoveDependency("", "");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Replacing dependees with an empty string shouldn't do anything
        /// </summary>
        [TestMethod()]
        public void ReplacingDependeesWithEmptyString()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.ReplaceDependees("", new List<string>());
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Replacing dependents with an empty string shouldn't do anything
        /// </summary>
        [TestMethod()]
        public void ReplacingDependentsWithEmptyString()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.ReplaceDependents("", new List<string>());
            Assert.AreEqual(1, t.Size);
        }
    }
}
