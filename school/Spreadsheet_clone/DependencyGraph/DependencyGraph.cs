/// <summary>
/// Author:    Isaac Stagg, skeleton written by Joe Zachary (September 2013) and updated by Daniel Kopta.
/// Partner:   None
/// Date:      1/25/2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file creates a dependency graph that links cells together. This enables us to keep track of the
/// order to compute cells in our spreadsheet. Some cells need to be computed before others so they can
/// pass their value onwards. This file creates a way to keep track of and edit these dependencies.
/// </summary>
namespace SpreadsheetUtilities {

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    ///
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a
    /// set, and the element is already in the set, the set remains unchanged.
    ///
    /// Given a DependencyGraph DG:
    ///
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)
    ///
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on)
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph {

        // a node that is depended upon
        private Dictionary<string, HashSet<string>> dependees;

        // a node that depends on another node
        private Dictionary<string, HashSet<string>> dependents;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// Dependees: key is the given node and value is any node that needs to be calculated before the key.
        /// Dependents: key is the given node and value is any node that needs to be calculated after the key.
        /// </summary>
        public DependencyGraph() {
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        /// <returns>Self explanatory</returns>
        public int Size {
            get {
                // couldn't get O(1) implementation working in time
                int count = 0;
                foreach (var key in dependees.Keys) {
                    count += dependees[key].Count;
                }
                return count;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        /// <param name="s">The node to check dependees of</param>
        /// <returns>The number of dependees the given node, s, has.</returns>
        public int this[string s] {
            get {
                if (dependees.ContainsKey(s)) return dependees[s].Count;
                return 0;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        /// <param name="s">The node to check if it has dependents</param>
        /// <returns>Self explanatory</returns>
        public bool HasDependents(string s) {
            if (dependents.ContainsKey(s) && dependents[s].Count > 0) return true;
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        /// <param name="s">The node to check if it has dependees</param>
        /// <returns>Self explanatory</returns>
        public bool HasDependees(string s) {
            if (dependees.ContainsKey(s) && dependees[s].Count > 0) return true;
            return false;
        }

        /// <summary>
        /// Helper method to consolidate code between GetDependees and GetDependents
        /// </summary>
        /// <param name="dict">Either Dependents or Dependees</param>
        /// <param name="s">The node to get the dependents/dependees of</param>
        /// <returns></returns>
        private IEnumerable<string> GetDependeesDependentsHelper(Dictionary<string, HashSet<string>> dict, string s) {
            if (dict.ContainsKey(s)) {
                return dict[s];
            } else {
                return new HashSet<string>();
            }
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        /// <param name="s">The node to get the dependents of</param>
        /// <returns>A hash set of the dependents of s. If there are no dependents then an empty hash set is returned.</returns>
        public IEnumerable<string> GetDependents(string s) {
            return GetDependeesDependentsHelper(dependents, s);
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        /// <param name="s">The node to get the dependees of</param>
        /// <returns>A hash set of the dependees of s. If there are no dependees then an empty hash set is returned.</returns>
        public IEnumerable<string> GetDependees(string s) {
            return GetDependeesDependentsHelper(dependees, s);
        }

        /// <summary>
        /// Checks if s is in given dictionary. If it is not, add s to dependee dictionary with no value
        /// </summary>
        /// <param name="dict">Either the dependee or dependent dictionary</param>
        /// <param name="s">The key to add</param>
        private void AddIfAbsentHelper(Dictionary<string, HashSet<string>> dict, string s) {
            if (!dict.ContainsKey(s)) dict.Add(s, new HashSet<string>());
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        ///
        /// <para>This should be thought of as:</para>
        ///
        ///   t depends on s
        ///
        /// s and t cannot be null or empty
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>
        public void AddDependency(string s, string t) {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t)) return;

            AddIfAbsentHelper(dependees, s);
            AddIfAbsentHelper(dependents, t);

            //add t as value to dependent[s]
            if (!dependents.ContainsKey(s)) {
                dependents.Add(s, new HashSet<string> { t });
            } else if (!dependents[s].Contains(t)) {
                dependents[s].Add(t);
            }

            //add s as value to dependee[t]
            if (!dependees.ContainsKey(t)) {
                dependees.Add(t, new HashSet<string> { s });
            } else if (!dependees[t].Contains(s)) {
                dependees[t].Add(s);
            }
        }

        /// <summary>
        /// Used after removing a dependency. Sometimes a node is left behind but it has no dependees or
        /// dependents. In this case, this method will check to make sure it fits these conditions then
        /// it will remove the node form the data structure to free up memory.
        /// </summary>
        private void RemoveCurrentNodesWithNoDependencies(string s, string t) {
            if (dependees.ContainsKey(s) && dependees[s].Count == 0 && !dependents.ContainsKey(s)) dependees.Remove(s);
            if (dependents.ContainsKey(s) && dependents[t].Count == 0 && !dependees.ContainsKey(t)) dependents.Remove(t);

            // s has no dependees or dependents
            if (dependees.ContainsKey(s) && dependents.ContainsKey(s) && dependees[s].Count == 0 && dependents[s].Count == 0) {
                dependents.Remove(s);
                dependees.Remove(s);
            }

            // t has no dependees or dependents
            if (dependees.ContainsKey(t) && dependents.ContainsKey(t) && dependees[t].Count == 0 && dependents[t].Count == 0) {
                dependents.Remove(t);
                dependees.Remove(t);
            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists.
        /// s and t cannot be null.
        /// </summary>
        /// <param name="s">The dependee of the pair</param>
        /// <param name="t">The dependent of the pair</param>
        public void RemoveDependency(string s, string t) {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t)) return;

            if (dependees.ContainsKey(t) && dependents.ContainsKey(s)) {
                if (dependees[t].Count > 1) {
                    dependees[t].Remove(s);
                } else {
                    dependees.Remove(t);
                }

                if (dependents[s].Count > 1) {
                    dependents[s].Remove(t);
                } else {
                    dependents.Remove(s);
                }

                RemoveCurrentNodesWithNoDependencies(s, t);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// Inputs cannot be null.
        /// </summary>
        /// <param name="s">The dependee of the pair who's dependents will be replaced</param>
        /// <param name="newDependents">hash set of new dependents to be added under s</param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents) {
            if (string.IsNullOrEmpty(s)) return;

            if (dependents.ContainsKey(s)) {
                HashSet<string> temp = dependents[s];
                foreach (var r in temp) {
                    if (dependees.ContainsKey(r)) dependees[r].Remove(s);
                }
                dependents[s] = new HashSet<string>();
                foreach (string newDependent in newDependents) {
                    dependents[s].Add(newDependent);
                    AddIfAbsentHelper(dependents, newDependent);
                    if (!dependees.ContainsKey(newDependent)) {
                        dependees.Add(newDependent, new HashSet<string> { s });
                    } else {
                        dependees[newDependent].Add(s);
                    }
                }
                foreach (var r in temp) {
                    RemoveCurrentNodesWithNoDependencies(s, r);
                }
            } else {
                dependents.Add(s, new HashSet<string>(newDependents));
                foreach (string newDependent in newDependents) {
                    if (dependees.ContainsKey(newDependent)) {
                        dependees[newDependent].Add(s);
                    } else {
                        dependees.Add(newDependent, new HashSet<string> { s });
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each
        /// t in newDependees, adds the ordered pair (t,s).
        /// Inputs cannot be null.
        /// </summary>
        /// <param name="s">The dependent of the pair who's dependees will be replaced</param>
        /// <param name="newDependees">A hash set of dependees that will replace s's dependees</param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees) {
            if (string.IsNullOrEmpty(s)) return;

            if (dependees.ContainsKey(s)) {
                HashSet<string> temp = new HashSet<string>(dependees[s]);
                foreach (string r in temp) {
                    dependees[s].Remove(r);
                    dependents[r].Remove(s);
                }
                dependees[s] = new HashSet<string>(newDependees);
                foreach (string newDependee in newDependees) {
                    dependents[newDependee].Add(s);
                }
                foreach (string r in temp) {
                    RemoveCurrentNodesWithNoDependencies(s, r);
                }
            } else {
                dependees.Add(s, new HashSet<string>(newDependees));
                foreach (string newDependee in newDependees) {
                    if (dependents.ContainsKey(s)) {
                        dependents[newDependee].Add(s);
                    } else {
                        dependents.Add(newDependee, new HashSet<string> { s });
                    }
                }
            }
        }
    }
}