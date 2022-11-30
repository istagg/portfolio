/// <summary> 
/// Author:    Isaac Stagg, CS 3500 (used some tests from assignment 1 grading tests)
/// Partner:   None
/// Date:      1/29/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// This is a unit testing file for the library Formula. 44 tests cover 100% of the library in its
/// current state.
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTests
{
    /// <summary>
    /// A class of tests for the Formula class
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        /* --------- CONSTRUCTOR TESTING --------- */

        /// <summary>
        /// Having an empty formula should throw a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyFormula()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Having a formula with only whitespace should throw a fomula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WhiteSpaceFormula()
        {
            Formula f = new Formula(" ");
        }

        /// <summary>
        /// Having a token with one number shouldn't throw an exception
        /// </summary>
        [TestMethod]
        public void OneTokenFormula()
        {
            Formula f = new Formula("5");
        }

        /// <summary>
        /// Constructing a simple formula with a valid variable shouldn't throw anything.
        /// </summary>
        [TestMethod]
        public void SimpleVarFormula()
        {
            Formula f = new Formula("5+a3", s => s.ToUpper(), s => true);
        }

        /// <summary>
        /// Having a formula with one operator should throw a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OneOperatorFormula()
        {
            Formula f = new Formula("+");
        }

        /// <summary>
        /// Having a formula with one number and one operator should throw a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OneNumOneOpFormula()
        {
            Formula f = new Formula("5+");
        }

        /// <summary>
        /// Having a formula with an unequal amount of right and left parentheses should throw a
        /// formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnequalParen()
        {
            Formula f = new Formula("((5+5)+7");
        }

        /// <summary>
        /// Having a formula with more closing parentheses than opening parentheses at any time should throw
        /// a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreClosingParen()
        {
            Formula f = new Formula("(5+3))");
        }

        /// <summary>
        /// Having a formula with an operator and a closing parentheses immediately after it should throw
        /// a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorThenClosingParen()
        {
            Formula f = new Formula("(3+)");
        }

        /// <summary>
        /// Having a formula with a number and no operator immediately before or after it should throw a
        /// formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NumNoOp()
        {
            Formula f = new Formula("(3*2)5");
        }

        /// <summary>
        /// If all the variables in the function are not valid then a formula format exception should be
        /// thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void AllVarNotValid()
        {
            Formula f = new Formula("5+A1", s => s.ToUpper(), s => false);
        }

        /// <summary>
        /// Multi-letter variables should throw an error
        /// thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiLetterVar()
        {
            Formula f = new Formula("yy", s => s.ToUpper(), s => false);
        }

        /// <summary>
        /// Testing a whole bunch of variable names that should be valid under the minimum requirements
        /// </summary>
        [TestMethod]
        public void ValidVariableNames()
        {
            Formula f1 = new Formula("_a3_a4");
            Formula f2 = new Formula("aaaaa3");
            Formula f3 = new Formula("a_2");
            Formula f4 = new Formula("______a2");
        }

        /// <summary>
        /// Testing variable that does not meet minimum naming requirements
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidVariableName()
        {
            Formula f1 = new Formula("0_a3_a4");
        }

        /// <summary>
        /// Testing another variable that does not meet minimum naming requirements
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void AnotherInvalidVariableName()
        {
            Formula f1 = new Formula("&a3");
        }

        /// <summary>
        /// Unary negative numbers are not allowed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnaryNegative()
        {
            Formula f1 = new Formula("-3");
        }




        /* --------- EVALUATE METHOD TESTING --------- */

        /// <summary>
        /// A simple addition formula should return the correct value
        /// </summary>
        [TestMethod]
        public void SimpleAdditionFormula()
        {
            Formula f = new Formula("2+7");
            Assert.AreEqual(9.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// A simple formula with spaces should return the correct value
        /// </summary>
        [TestMethod]
        public void SimpleFormulaWithSpaces()
        {
            Formula f = new Formula("2 + 7");
            Assert.AreEqual(9.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// A simple subtraction formula should return the correct value
        /// </summary>
        [TestMethod]
        public void SimpleSubtractionFormula()
        {
            Formula f = new Formula("9-7");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// A simple multiplication formula should return the correct value
        /// </summary>
        [TestMethod]
        public void SimpleMultiplicationFormula()
        {
            Formula f = new Formula("3*2");
            Assert.AreEqual(6.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// A simple division formula should return the correct value
        /// </summary>
        [TestMethod]
        public void SimpleDivisionFormula()
        {
            Formula f = new Formula("4/2");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// Explicitly dividing by zero should return a FormulaError
        /// </summary>
        [TestMethod]
        public void ExplicitDivideByZero()
        {
            Formula f = new Formula("2/0");
            Assert.AreEqual(typeof(FormulaError), f.Evaluate(s => 0).GetType());
        }

        /// <summary>
        /// Implicitly dividing by zero should return a FormulaError
        /// </summary>
        [TestMethod]
        public void ImplicitDivideByZero()
        {
            Formula f = new Formula("2/(2-2)");
            Assert.AreEqual(typeof(FormulaError), f.Evaluate(s => 0).GetType());
        }

        /// <summary>
        /// Addition with a variable should correctly lookup the variable value and calculate the output
        /// correctly
        /// </summary>
        [TestMethod]
        public void TestArithmeticWithVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.AreEqual(6.0, f.Evaluate(s => 4));
        }

        /// <summary>
        /// Addition with an unknown variable should return a formula error
        /// </summary>
        [TestMethod]
        public void UnknownVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.AreEqual(typeof(FormulaError), f.Evaluate(s => throw new ArgumentException("Unknown variable")).GetType());
        }

        /// <summary>
        /// Testing multiple operations
        /// </summary>
        [TestMethod]
        public void TestLeftToRight()
        {
            Formula f = new Formula("2*6+3");
            Assert.AreEqual(15.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// Testing order of operations
        /// </summary>
        [TestMethod]
        public void TestOrderOperations()
        {
            Formula f = new Formula("2+6*3");
            Assert.AreEqual(20.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// basic test with parentheses
        /// </summary>
        [TestMethod]
        public void TestParenthesesTimes()
        {
            Formula f = new Formula("(2+6)*3");
            Assert.AreEqual(24.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// See name
        /// </summary>
        [TestMethod]
        public void TestOperatorAfterParenthesis()
        {
            Formula f = new Formula("(1*1)-2/2");
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// Testing complex order of operations and parentheses
        /// </summary>
        [TestMethod]
        public void TestComplexAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }
        
        /// <summary>
        /// Testing complex order of operations, parentheses and multiple variables
        /// </summary>
        [TestMethod]
        public void TestComplexMultiVar()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            double x = 20;
            double y = 7;
            double answer = x / y;
            Assert.AreEqual((double) 8 - answer, f.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        /// <summary>
        /// If variable holds value zero and is denominator of fraction then a formula error should be
        /// returned
        /// </summary>
        [TestMethod]
        public void VariableDivideByZero()
        {
            Formula f = new Formula("4/A3");
            Assert.AreEqual(typeof(FormulaError), f.Evaluate(s => 0).GetType());
        }

        /// <summary>
        /// Testing the double.parse on scientific notation
        /// </summary>
        [TestMethod]
        public void ScientificNotationParsing()
        {
            Formula f = new Formula("1e3");
            Assert.AreEqual(1000.0, f.Evaluate(s => 0));
        }

        /// <summary>
        /// Testing one of everything (mult, divide, variable, sci notation, etc)
        /// </summary>
        [TestMethod]
        public void JackOfAllTests()
        {
            Formula f = new Formula("1+2*(1e1/a1)-2", s => s.ToUpper(), s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => 10));
        }




        /* --------- GetVariables Testing --------- */

        /// <summary>
        /// Testing the get variables test without normalizing
        /// </summary>
        [TestMethod]
        public void BasicGetVariablesTest()
        {
            Formula f = new Formula("x+Y+z");
            IEnumerator<string> e = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("x", e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("Y", e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("z", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Testing the get variables method with normalizing. should ignore repeats
        /// </summary>
        [TestMethod]
        public void NormalizedDuplicateGetVariablesTest()
        {
            Formula f = new Formula("x+X*z", s => s.ToUpper(), s => true);
            IEnumerator<string> e = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("X", e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("Z", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Testing the get variables method without normalizing. should separate capitalized vs not capitalized
        /// </summary>
        [TestMethod]
        public void DuplicateButDifferentCaseGetVariablesTest()
        {
            Formula f = new Formula("x+X*z");
            IEnumerator<string> e = f.GetVariables().GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("x", e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("X", e.Current);
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("z", e.Current);
            Assert.IsFalse(e.MoveNext());
        }



        /* --------- ToString Testing --------- */

        /// <summary>
        /// Testing the to string method without any normalizing. should get rid of spaces
        /// </summary>
        [TestMethod]
        public void SimpleToString()
        {
            Formula f = new Formula("x + Y");
            Assert.AreEqual("x+Y", f.ToString());
        }

        /// <summary>
        /// Testing the to string method with normalizing. should get rid of spaces
        /// </summary>
        [TestMethod]
        public void NormalizedToString()
        {
            Formula f = new Formula("x + y", s => s.ToUpper(), s => true);
            Assert.AreEqual("X+Y", f.ToString());
        }




        /* ---------- Testing Various Equals ---------- */

        /// <summary>
        /// Testing the equals method with multiple zeros after decimal vs few
        /// </summary>
        [TestMethod]
        public void EqualsManyDecimals()
        {
            Formula f = new Formula("2.1 + x7");
            Assert.IsTrue(f.Equals(new Formula("2.100 + x7")));
        }

        /// <summary>
        /// equals method for variables in a different order should return false
        /// </summary>
        [TestMethod]
        public void EqualsDifferentOrder()
        {
            Formula f = new Formula("x1+y2");
            Assert.IsFalse(f.Equals(new Formula("y2+x1")));
        }

        /// <summary>
        /// upper and lowercase shouldn't equal if no normalization is occurring
        /// </summary>
        [TestMethod]
        public void EqualsNoNormalizing()
        {
            Formula f = new Formula("x1+y2");
            Assert.IsFalse(f.Equals(new Formula("X1+Y2")));
        }

        /// <summary>
        /// extra whitespace shouldn't affect formulas being equal
        /// </summary>
        [TestMethod]
        public void WhitespaceEquals()
        {
            Formula f = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Assert.IsTrue(f.Equals(new Formula("X1  +  Y2")));
        }

        /// <summary>
        /// Testing == operator does same operation as .Equals()
        /// </summary>
        [TestMethod]
        public void OperatorEquals()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");
            Assert.IsTrue(f1 == f2);
        }

        /// <summary>
        /// Testing != operator does same operation as should return true when different formulas are used
        /// </summary>
        [TestMethod]
        public void OperatorNotEquals()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("y2+x1");
            Assert.IsTrue(f1 != f2);
        }

        /// <summary>
        /// Testing that equality works with normalization and variables
        /// </summary>
        [TestMethod]
        public void FromulasWithVariablesEqual()
        {
            Formula f1 = new Formula("x1 + y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1+Y2");
            Assert.IsTrue(f1 == f2);
        }

        /// <summary>
        /// Testing equality of different types should return false
        /// </summary>
        [TestMethod]
        public void DifferentTypesEquality()
        {
            Formula f1 = new Formula("5");
            Assert.IsFalse(f1.Equals("test"));
        }

        /// <summary>
        /// Testing equality of different types with the same content should return false
        /// </summary>
        [TestMethod]
        public void DifferentTypesSameContent()
        {
            Formula f1 = new Formula("5");
            Assert.IsFalse(f1.Equals("5.0"));
        }

        /// <summary>
        /// Testing equality of a formula and null should return false
        /// </summary>
        [TestMethod]
        public void NullEquals()
        {
            Formula f1 = new Formula("5*2");
            Assert.IsFalse(f1.Equals(null));
        }




        /* ---------- Testing HashCode ---------- */

        /// <summary>
        /// Testing same formulas having same hash code
        /// </summary>
        [TestMethod]
        public void SameFormulaHashCode()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1 + Y2");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        /// <summary>
        /// Testing different formulas (varying by capitalization) having different hash code
        /// </summary>
        [TestMethod]
        public void DifferentFormulaHashCode()
        {
            Formula f1 = new Formula("x1+y2");
            Formula f2 = new Formula("X1 + Y2");
            Assert.IsFalse(f1.GetHashCode() == f2.GetHashCode());
        }

        /// <summary>
        /// Testing hash codes are equal but with scientific notation and normalization
        /// </summary>
        [TestMethod]
        public void SameFormulaHashCodeSciNotation()
        {
            Formula f1 = new Formula("x1+1e1");
            Formula f2 = new Formula("X1 + 10", s => s.ToLower(), s => true);
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }
    }
}