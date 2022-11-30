/// <summary>
/// Author:    Isaac Stagg
/// Partner:   None
/// Date:      2/6/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
/// 
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
/// 
/// File Contents
/// This is a unit testing file for the library Spreadsheet. 17 tests cover 100% of the library.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests the GetNamesOfAllNonemptyCells method with a single variable
        /// </summary>
        [TestMethod]
        public void SingleCellGetNames()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5.0");
            IEnumerator<string> e = s.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("A1", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Testing the GetContents method on a double should return the value that the cell was defined with
        /// </summary>
        [TestMethod]
        public void SingleCellGetContentsDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(2.0, s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing the GetContents method on a string should return the value that the cell was defined with.
        /// GetContents should also be case sensitive.
        /// </summary>
        [TestMethod]
        public void SingleCellGetContentsString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("A1"));
            Assert.AreNotEqual("Hello", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing the GetContents method on a Formula should return the value that the cell was defined
        /// with.
        /// </summary>
        [TestMethod]
        public void SingleCellGetContentsFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=5+2");
            Assert.AreEqual(new Formula("5+2"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing the GetContents method on an empty cell should return an empty string.
        /// </summary>
        [TestMethod]
        public void GetContentsEmptyCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing the GetContents method on a cell with an invalid name should throw an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetContentsBadName()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents("0");
        }

        /// <summary>
        /// Testing the GetNamesNonemptyCells method on an empty spreadsheet should return an empty list.
        /// </summary>
        [TestMethod]
        public void GetCellsSpreadsheetEmpty()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            IEnumerator<string> e = s.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Setting the contents of a cell that already exists to something else shouldn't cause any errors
        /// </summary>
        [TestMethod]
        public void ChangeCellContents()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            Assert.AreEqual(1.0, s.GetCellContents("A1"));
            s.SetContentsOfCell("A1", "5.0");
            Assert.AreEqual(5.0, s.GetCellContents("A1"));
        }

        /// <summary>
        /// Setting the contents of a cell that already exists to a different type of content shouldn't
        /// cause any errors
        /// </summary>
        [TestMethod]
        public void ChangeCellContentsType()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            Assert.AreEqual(1.0, s.GetCellContents("A1"));
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Using the SetContentsOfCell method should return a list of everything dependent upon that cell.
        /// </summary>
        [TestMethod]
        public void CellContentsDependentsDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("C1", "=B1+A1");
            s.SetContentsOfCell("B1", "=A1*2");
            IEnumerator<string> e = s.SetContentsOfCell("A1", "1.0").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("A1", e.Current);
            Assert.IsTrue(e.MoveNext());
            string s1 = e.Current;
            Assert.IsTrue((s1 == "B1") || (s1 == "C1"));
            Assert.IsTrue(e.MoveNext());
            s1 = e.Current;
            Assert.IsTrue((s1 == "B1") || (s1 == "C1"));
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Using the SetContentsOfCell method should return a list of everything dependent upon that cell.
        /// </summary>
        [TestMethod]
        public void CellContentsDependentsString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=2*2");
            CollectionAssert.AreEqual(new List<string>() {"A1"}, new List<string>(s.SetContentsOfCell("A1", "hello")));
        }

        /// <summary>
        /// Using the SetContentsOfCell method should return a list of everything dependent upon that cell.
        /// </summary>
        [TestMethod]
        public void CellContentsDependentsFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=A1*2");
            s.SetContentsOfCell("C1", "=B1+A1");
            CollectionAssert.AreEqual(new List<string>() {"A1", "B1", "C1"}, new List<string>(s.SetContentsOfCell("A1", "=2+2")));
        }

        /// <summary>
        /// If a circular dependency is detected, a circular exception should be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void BasicCircular()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+2");
            s.SetContentsOfCell("B1", "=A1*3");
        }

        /// <summary>
        /// Testing a more complicated series of formulas that result in a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void ComplexCircular()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+2");
            s.SetContentsOfCell("B1", "=C1/9");
            s.SetContentsOfCell("D1", "=A1*B1");
            s.SetContentsOfCell("C1", "=D1+1");
        }

        /// <summary>
        /// Replacing a cell that contains a formula with another formula should erase all old dependencies
        /// and add new ones.
        /// </summary>
        [TestMethod]
        public void ReplaceFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1*2");
            s.SetContentsOfCell("B1", "=C1/2");
            s.SetContentsOfCell("C1", "2.0");
            s.SetContentsOfCell("B1", "=5+1");
            s.SetContentsOfCell("B1", "=C1/2");
        }

        /// <summary>
        /// switching a cell's contents from a double to a string then formula should not cause any issues
        /// </summary>
        [TestMethod]
        public void SwitchingCellTypeMultipleTimes()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hi");
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A1", "=6/2");
            Assert.AreEqual(new Formula("6 / 2"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// setting the cell contents using string method with an invalid name should throw an invalid name
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameStringCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("&", "HI");
        }

        /// <summary>
        /// setting the cell contents using double method with an invalid name should throw an invalid name
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameDoubleCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("8", "6.0");
        }

        /// <summary>
        /// setting the cell contents using formula method with an invalid name should throw an invalid name
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameFormulaCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("@", "=2+2");
        }

        /// <summary>
        /// During lab 5. Sending in an empty cell name should throw an invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptyNameTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("", "Hello");
        }

        /// <summary>
        /// From lab 5. Sending in an empty string as the contents should not create a new cell.
        /// </summary>
        [TestMethod]
        public void EmptyContents()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// From lab 5. Testing circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyExceptionTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1");
            s.SetContentsOfCell("B1", "=A1");
        }

        /// <summary>
        /// From lab 5. Changing a cell to something invalid should keep the old cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularDependencyExceptionRevertTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "Hello");
                s.SetContentsOfCell("A1", "=A1");
            }
            catch (CircularException e)
            {
                Assert.AreEqual("Hello", s.GetCellContents("A1"));
                throw e;
            }
        }

        /// <summary>
        /// When changing a cell with a formula to one without a formula, all dependencies should be removed.
        /// </summary>
        [TestMethod]
        public void FormulaToTextCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "5.0");
            s.SetContentsOfCell("A1", "=B1*2");
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("A1"));
        }

        /// <summary>
        /// When changing a cell with a formula to one without a formula, all dependencies should be removed.
        /// </summary>
        [TestMethod]
        public void FormulaToDoubleCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "5.0");
            s.SetContentsOfCell("A1", "=B1*2");
            s.SetContentsOfCell("A1", "1.0");
            Assert.AreEqual(1.0, s.GetCellContents("A1"));
        }

        /// <summary>
        /// A cell shouldn't be able to depend on itself
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CellDependsOnItself()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A1+A1");
        }

        /// <summary>
        /// Passing in an empty formula should throw a formula format exception from last assignment
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=");
        }

        /// <summary>
        /// Testing a more complex arrangement of formulas where the dependency tree is larger. Looking at the
        /// output of the SetContentsOfCell method.
        /// </summary>
        [TestMethod]
        public void ComplicatedSetContentsOfCellReturn()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+C1");
            s.SetContentsOfCell("B1", "=C1*2");
            CollectionAssert.AreEqual(new List<string>() {"C1", "B1", "A1"}, new List<string>(s.SetContentsOfCell("C1", "2.0")));
        }

        /// <summary>
        /// Testing that removing one cell only removes the immediate dependencies.
        /// </summary>
        [TestMethod]
        public void RemovingDependencies()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+C1");
            s.SetContentsOfCell("B1", "=D1*2");
            s.SetContentsOfCell("C1", "=E1/2");
            s.SetContentsOfCell("D1", "2.0");
            s.SetContentsOfCell("E1", "5.0");
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Setting a cell with a formula to another formula removes the old dependencies and adds new ones
        /// </summary>
        [TestMethod]
        public void ChangingDependencies()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1*C1");
            s.SetContentsOfCell("A1", "=D1+E1");
        }

        /// <summary>
        /// Getting the contents of a cell that hasn't been set yet should return an empty string.
        /// </summary>
        [TestMethod]
        public void GetContentsEmpty()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("Z9"));
        }

        /// <summary>
        /// Setting a cell to an empty string should not add it to data structure
        /// </summary>
        [TestMethod]
        public void SetEmptyString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Simple test for GetValue method for a double.
        /// </summary>
        [TestMethod]
        public void GetValueDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(2.0, (double) s.GetCellValue("A1"));
        }

        /// <summary>
        /// Simple test for GetValue method for a string.
        /// </summary>
        [TestMethod]
        public void GetValueString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellValue("A1"));
        }

        /// <summary>
        /// Simple test for GetValue method for a formula with no variables.
        /// </summary>
        [TestMethod]
        public void GetValueFormulaNoVariables()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=2*4");
            Assert.AreEqual(8.0, (double) s.GetCellValue("A1"));
        }

        /// <summary>
        /// Simple test for GetValue method for a formula with variables.
        /// </summary>
        [TestMethod]
        public void GetValueFormulaWithVariables()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "6.0");
            s.SetContentsOfCell("B1", "3.0");
            s.SetContentsOfCell("C1", "=A1 / B1");
            Assert.AreEqual(2.0, (double) s.GetCellValue("C1"));
        }

        /// <summary>
        /// Test for GetValue method for a formula with variables that change.
        /// </summary>
        [TestMethod]
        public void GetValueFormulaChangingVariables()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "6.0");
            s.SetContentsOfCell("B1", "3.0");
            s.SetContentsOfCell("C1", "=A1 / B1");
            s.SetContentsOfCell("B1", "2.0");
            Assert.AreEqual(3.0, (double)s.GetCellValue("C1"));
        }

        /// <summary>
        /// Test for GetValue method for a formula with a variable that holds a string.
        /// </summary>
        [TestMethod]
        public void GetValueFormulaStringVariable()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hi");
            s.SetContentsOfCell("B1", "=A1*2");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("B1").GetType());
        }

        /// <summary>
        /// When changing a cell with a FormulaError to something that doesn't cause an error calculates the
        /// new value.
        /// </summary>
        [TestMethod]
        public void ChangingCellContentsFromError()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "0");
            s.SetContentsOfCell("B1", "=1/A1");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("B1").GetType());
            s.SetContentsOfCell("A1", "1");
            Assert.AreEqual(1.0, (double) s.GetCellValue("B1"));
        }

        /// <summary>
        /// Testing that a spreadsheet with no normalization differentiates between cases.
        /// </summary>
        [TestMethod]
        public void NoNormalizing()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("a1", "hello");
            Assert.AreEqual(1.0, s.GetCellValue("A1"));
            Assert.AreEqual("hello", s.GetCellValue("a1"));
        }

        [TestMethod]
        public void Normalizing()
        {
            AbstractSpreadsheet s = new Spreadsheet(_ => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "hi");
            Assert.AreEqual("hi", s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing other constructors.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ThreeArgumentConstructorTest()
        {
            AbstractSpreadsheet s = new Spreadsheet(s => s.Equals("A1"), s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "1.0");
            s.SetContentsOfCell("b1", "hi");
        }

        /// <summary>
        /// Instantiating many formulas that all depend on one variable that hasn't been instantiated yet
        /// should populate all cells' values with FormulaError. Once the variable has been added everything
        /// should be calculated.
        /// </summary>
        [TestMethod]
        public void ChangingVariable()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=A1+C1");
            s.SetContentsOfCell("C1", "=A1 * 2");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("B1").GetType());
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("C1").GetType());
            s.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(4.0, (double)s.GetCellValue("C1"));
            Assert.AreEqual(6.0, (double)s.GetCellValue("B1"));
        }

        /// <summary>
        /// Creates a regular spreadsheet with some contents. For testing purposes.
        /// </summary>
        private AbstractSpreadsheet SaveWorkingSpreadsheet(string filepath)
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "test");
            s.SetContentsOfCell("B1", "2.0");
            s.SetContentsOfCell("C1", "=B1*2");
            s.Save(filepath);
            return s;
        }

        /// <summary>
        /// Testing saving a spreadsheet to XML file
        /// </summary>
        [TestMethod]
        public void TestSave()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(false, s.Changed);
            s.SetContentsOfCell("A1", "test");
            s.SetContentsOfCell("B1", "2.0");
            s.SetContentsOfCell("C1", "=B1*2");
            Assert.AreEqual(true, s.Changed);
            s.Save("Test1.xml");
            Assert.AreEqual(false, s.Changed);
            Assert.AreEqual("default", s.GetSavedVersion("Test1.xml"));
            File.Delete("Test1.xml");
        }

        /// <summary>
        /// Testing loading a spreadsheet from a saved file
        /// </summary>
        [TestMethod]
        public void LoadFromSave()
        {
            string filepath = "Test.xml";
            SaveWorkingSpreadsheet(filepath);
            AbstractSpreadsheet s = new Spreadsheet(filepath, _ => true, s => s, "default");
            Assert.AreEqual(false, s.Changed);
            Assert.AreEqual("test", s.GetCellValue("A1"));
            Assert.AreEqual(2.0, (double)s.GetCellValue("B1"));
            Assert.AreEqual(4.0, (double)s.GetCellValue("C1"));
            File.Delete(filepath);
        }

        /// <summary>
        /// Testing that loading a save that has a different version throws a read write exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadFromInvalidSaveVersion()
        {
            string filepath = "Test.xml";
            SaveWorkingSpreadsheet(filepath);
            try
            {
                AbstractSpreadsheet s = new Spreadsheet(filepath, _ => true, s => s, "1.0");
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete(filepath);
            }
        }

        /// <summary>
        /// Testing that loading a save from an invalid path throws a read write exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadFromInvalidPath()
        {
            AbstractSpreadsheet s = new Spreadsheet("/blah/blah/blah.xml", _ => true, s => s, "1.0");
        }

        /// <summary>
        /// Saving to a path that doesn't exist should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveNoPath()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.Save("/some/nonsense/path.xml");
        }

        /// <summary>
        /// Getting the file version from a path that doesn't exist should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetVersionWrongPath()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("/some/nonsense/path.xml");
        }

        /// <summary>
        /// Checking that this is an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetContents()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        /// <summary>
        /// Setting a cell to an empty string shouldn't create the cell
        /// </summary>
        [TestMethod]
        public void TestExplicitEmptySet()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Testing the order of set contents output
        /// </summary>
        [TestMethod]
        public void TestSetChain()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "6.0");
            s.SetContentsOfCell("A3", "=A2+A4");
            s.SetContentsOfCell("A4", "=A2+A5");
            CollectionAssert.AreEqual((System.Collections.ICollection)s.SetContentsOfCell("A5", "82.5"), new List<string>() { "A5", "A4", "A3", "A1" });
        }

        /// <summary>
        /// The normalization should happen on every name that goes into the spreadsheet
        /// </summary>
        [TestMethod]
        public void NormalizeOtherNames()
        {
            AbstractSpreadsheet s = new Spreadsheet(_ => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "hello");
            Assert.AreEqual("hello", s.GetCellValue("A1"));
            s.SetContentsOfCell("B1", "2.0");
            Assert.AreEqual(2.0, (double)s.GetCellValue("b1"));
        }

        [TestMethod]
        public void EmptyStringDependenciesTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1");
            s.SetContentsOfCell("B1", "2.0");
            Assert.AreEqual(2.0, (double)s.GetCellValue("A1"));
            s.SetContentsOfCell("B1", "");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("A1").GetType());
        }

        /// <summary>
        /// Getting the value of a cell that isn't in the spreadsheet should return an empty string
        /// </summary>
        [TestMethod]
        public void GetCellValueInvalidName()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellValue("Z9"));
        }

        /// <summary>
        /// when loading from a file that has tags that aren't of the form cell/cells/spreadsheet/name/contents
        /// a read write exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadSaveInvalidFormat()
        {
            XDocument doc = new(
                new XElement("spreadsheet",
                    new XAttribute("version", "default"),
                    new XElement("cell",
                        new XElement("name", "A1"),
                        new XElement("contents", "5.0"),
                        new XElement("age", "10")
                    )
                )
            );
            doc.Save("Test2.xml");
            try
            {
                AbstractSpreadsheet s = new Spreadsheet("Test2.xml", _ => true, s => s.ToUpper(), "default");
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete("Test2.xml");
            }
        }

        /// <summary>
        /// when loading from a file that has a cell that is missing contents then a read write
        /// exception should be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadSaveMissingContents()
        {
            XDocument doc = new(
                new XElement("spreadsheet",
                    new XAttribute("version", "default"),
                    new XElement("cell",
                        new XElement("name", "A1"),
                        new XElement("contents", "test")
                    ),
                    new XElement("cell",
                        new XElement("name", "B1")
                    )
                )
            );
            doc.Save("Test3.xml");
            try
            {
                AbstractSpreadsheet s = new Spreadsheet("Test3.xml", _ => true, s => s.ToUpper(), "default");
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete("Test3.xml");
            }
        }

        /// <summary>
        /// when loading from a file that is missing version information should throw read write
        /// exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadMissingVersion()
        {
            XDocument doc = new(
                new XElement("spreadsheet",
                    new XElement("cell",
                        new XElement("name", "A1"),
                        new XElement("contents", "test")
                    )
                )
            );
            doc.Save("Test4.xml");
            AbstractSpreadsheet s = new Spreadsheet();
            try
            {
                s.GetSavedVersion("Test4.xml");
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete("Test4.xml");
            }
        }

        /// <summary>
        /// when loading from a file that is missing the opening spreadsheet tag a read write exception
        /// is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadMissingOpeningTag()
        {
            XDocument doc = new(
                new XElement("test",
                    new XAttribute("version", "default"),
                    new XElement("cell",
                        new XElement("name", "A1"),
                        new XElement("contents", "test")
                    )
                )
            );
            doc.Save("Test5.xml");
            AbstractSpreadsheet s = new Spreadsheet();
            try
            {
                s.GetSavedVersion("Test5.xml");
            }
            catch
            {
                throw;
            }
            finally
            {
                File.Delete("Test5.xml");
            }
        }

        /// <summary>
        /// when loading from a filepath that is empty a read write exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadEmptyPath()
        {
            AbstractSpreadsheet s = new Spreadsheet("", _ => true, s => s, "1.0");
        }

        /// <summary>
        /// Stress test, lots of cells, saving, reading, etc.
        /// </summary>
        [TestMethod]
        public void StressTest()
        {
            string filepath = "StressTest.xml";
            AbstractSpreadsheet s1 = new Spreadsheet(_ => true, s => s.ToUpper(), "1.0");
            for (int i = 0; i < 500; i++)
            {
                s1.SetContentsOfCell("A" + i, i.ToString());
            }
            Assert.IsTrue(s1.Changed);
            s1.Save(filepath);
            Assert.IsFalse(s1.Changed);
            Assert.AreEqual("1.0", s1.GetSavedVersion(filepath));
            AbstractSpreadsheet s2 = new Spreadsheet(filepath, _ => true, s => s.ToUpper(), "1.0");
            Assert.IsFalse(s2.Changed);
            for (int i = 0; i < 500; i++)
            {
                Assert.AreEqual((double)s2.GetCellValue("A" + i), (double)i);
            }
            File.Delete(filepath);
        }
    }
}