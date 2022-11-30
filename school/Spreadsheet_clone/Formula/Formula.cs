/// <summary>
/// Author:    Isaac Stagg, skeleton written by Joe Zachary (September 2013) and updated by Daniel Kopta.
/// Partner:   None
/// Date:      1/29/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// This file creates an object from an inputted formula. This formula is verified to be syntactically correct
/// upon construction. The formula is also immutable so once it has been created it will always be syntactically
/// correct. Methods can be evaluated, compared, written as a string and hash codes can be generated.
/// </summary>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision
    /// floating-point syntax (without unary preceding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        private List<string> tokens = new List<string>();

        // creating a list of operators to check current token against
        private static List<string> opTokens = new List<string> { "(", ")", "+", "-", "*", "/" };
        private static IList<string> standardOpTokens = opTokens.AsReadOnly();

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true. 
        /// 
        /// </summary>
        /// <param name="formula">The inputted string containing a formula</param>
        public Formula(String formula) : this(formula, s => s, s => true) { }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        /// <param name="formula">An inputted string containing a formula</param>
        /// <param name="normalize">
        /// A delegate that converts all given variables into a standard notation. For example, capitalizing
        /// everything.
        /// </param>
        /// <param name="isValid">A delegate that checks if a variable name fits the naming convention</param>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            InstantiateTokensAndCheckForSyntaxErrors(formula);
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!standardOpTokens.Contains(tokens[i])) // then it is a var or num
                {
                    double num = 0.0;
                    if (!Double.TryParse(tokens[i], out num))
                    {
                        // The minimum requirements for a variable are any letter or underscore followed by
                        // any number of letters and/or digits and/or underscores.

                        // Might not need this if statement since GetTokens method separates out special
                        // characters which are then caught in my syntax checking method. will leave this
                        // just in case
                        if (Regex.IsMatch(tokens[i], @"[a-zA-Z_]+[0-9a-zA-Z_]*"))
                        {
                            string normalizedToken = normalize(tokens[i]);
                            if (!isValid(normalizedToken)) throw new FormulaFormatException(tokens[i] + " is not valid");
                            tokens[i] = normalizedToken;
                        }
                        else
                        {
                            throw new FormulaFormatException("Variable name does not meet minimum requirements");
                        }
                    }
                    else
                    {
                        // replaces the scientific notation with its numeric equivalent
                        tokens[i] = num.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Helper method for overlapping code between constructors. Instantiates the private token list.
        /// Checks the list of tokens for any syntax errors.
        /// </summary>
        /// <param name="formula">The inputted formula in which to check for syntax errors</param>
        /// <exception cref="FormulaFormatException">If a syntax error is found a FormulaFormatException is thrown</exception>
        private void InstantiateTokensAndCheckForSyntaxErrors(string formula)
        {
            tokens = GetTokens(formula).ToList();

            if (tokens.Count < 1) throw new FormulaFormatException("Must have one or more tokens");

            string firstToken = tokens.First<string>();
            if (!firstToken.Equals("(") && standardOpTokens.Contains(firstToken))
                throw new FormulaFormatException("Invalid starting token");

            string lastToken = tokens.Last<string>();
            if (!lastToken.Equals(")") && standardOpTokens.Contains(lastToken))
                throw new FormulaFormatException("Invalid ending token");

            int numOpeningParen = 0;
            int numClosingParen = 0;
            string prevToken = "";
            foreach (string token in tokens)
            {
                if (token.Equals("(")) numOpeningParen++;
                if (token.Equals(")")) numClosingParen++;
                if (numClosingParen > numOpeningParen)
                    throw new FormulaFormatException("Too many closing parentheses");
                if ((!prevToken.Equals(")") && standardOpTokens.Contains(prevToken)) || prevToken.Equals(""))
                {
                    if (!token.Equals("(") && standardOpTokens.Contains(token))
                        throw new FormulaFormatException("Invalid operator sequence (two operators after each other or empty parentheses etc)");
                }
                else
                {
                    if (!standardOpTokens.Contains(token) || token.Equals("("))
                        throw new FormulaFormatException("Invalid operator sequence (two operators after each other or empty parentheses etc)");
                }
                prevToken = token;
            }
            if (numClosingParen != numOpeningParen)
                throw new FormulaFormatException("Must have same number of opening and closing parentheses");
        }

        /// <summary>
        /// Functions as name indicates.
        /// </summary>
        /// <returns>
        /// Either a double containing computed answer or FormulaError if a divide by zero occurred
        /// </returns>
        private static object ComputeGivenOperation(String op, double val1, double val2)
        {
            if (op.Equals("+"))
            {
                return val1 + val2;
            }
            else if (op.Equals("-"))
            {
                return val2 - val1;
            }
            else if (op.Equals("*"))
            {
                return val1 * val2;
            }
            else // MUST be division. Method only called if op is +, -, *, /
            {
                if (val1 != 0)
                {
                    return val2 / val1;
                }
                else
                {
                    return new FormulaError("Cannot divide by 0");
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        /// <param name="lookup">
        /// A delegate that checks the value of a given variable or throws an exception if the variable has
        /// not been defined yet
        /// </param>
        /// <returns>
        /// If the calculation is successful a double value is returned. If the operation was not successful
        /// then a FormulaError object is returned.
        /// </returns>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<String> operatorStack = new Stack<String>();

            foreach (string token in tokens)
            {
                double num = 0;
                if (double.TryParse(token, out num)) // checks if token is a double. If it is, stored in num
                {
                    if (operatorStack.Count == 0)
                    {
                        valueStack.Push(num);
                    }
                    else if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                    {
                        object calculatedValue = ComputeGivenOperation(operatorStack.Pop(), num, valueStack.Pop());
                        if (calculatedValue.GetType().Equals(typeof(System.Double)))
                        {
                            valueStack.Push((double)calculatedValue);
                        }
                        else
                        {
                            return calculatedValue;
                        }
                    }
                    else
                    {
                        valueStack.Push(num);
                    }
                }
                else if (token.Equals("+") || token.Equals("-"))
                {
                    if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(token);
                    }
                    else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        double val1 = valueStack.Pop();
                        double val2 = valueStack.Pop();
                        // don't need to check for error being returned since we aren't using division operator
                        valueStack.Push((double) ComputeGivenOperation(operatorStack.Pop(), val1, val2));
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operatorStack.Push(token);
                    }
                }
                else if (token.Equals("*") || token.Equals("/") || token.Equals("("))
                {
                    operatorStack.Push(token);
                }
                else if (token.Equals(")"))
                {
                    if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        double val1 = valueStack.Pop();
                        double val2 = valueStack.Pop();
                        String op = operatorStack.Pop();
                        if (op.Equals("+") || op.Equals("-"))
                        {
                            // don't need to check for return being a divide by zero error since we aren't dividing
                            valueStack.Push((double) ComputeGivenOperation(op, val1, val2));
                        }
                    }
                    if (operatorStack.Count > 0 && operatorStack.Peek().Equals("("))
                    {
                        operatorStack.Pop();
                    }
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        double val1 = valueStack.Pop();
                        double val2 = valueStack.Pop();
                        object calculatedValue = ComputeGivenOperation(operatorStack.Pop(), val1, val2);
                        if (calculatedValue.GetType().Equals(typeof(System.Double)))
                        {
                            valueStack.Push((double)calculatedValue);
                        }
                        else
                        {
                            return calculatedValue;
                        }
                    }

                }
                else
                {
                    try
                    {
                        num = lookup(token);
                    }
                    catch (ArgumentException)
                    {
                        return new FormulaError("Variable has no value.");
                    }
                    if (operatorStack.Count == 0)
                    {
                        valueStack.Push(num);
                    }
                    else if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                    {
                        object calculatedValue = ComputeGivenOperation(operatorStack.Pop(), num, valueStack.Pop());
                        if (calculatedValue.GetType().Equals(typeof(System.Double)))
                        {
                            valueStack.Push((double)calculatedValue);
                        }
                        else
                        {
                            return calculatedValue;
                        }
                    }
                    else
                    {
                        valueStack.Push(num);
                    }
                }
            }
            if (operatorStack.Count == 0 && valueStack.Count == 1)
            {
                return valueStack.Pop();
            }
            else
            {
                if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                {
                    double val1 = valueStack.Pop();
                    double val2 = valueStack.Pop();
                    return ComputeGivenOperation(operatorStack.Pop(), val1, val2);
                }
                else
                { // just in case
                    return new FormulaError("Failure");
                }
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        /// <returns>
        /// An IEnumerable that contains all of the normalized variables given in the constructor. Only stores
        /// one copy of each variable unless there are different cases given in the normalization delegate.
        /// </returns>
        public IEnumerable<String> GetVariables()
        {
            List<String> variables = new List<String>();
            foreach (string token in tokens)
            {
                if (!standardOpTokens.Contains(token))
                {
                    // then it is either num, var or sci notation
                    if (!Double.TryParse(token, out _))
                    {
                        if (!variables.Contains(token)) variables.Add(token);
                    }
                }
            }
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        /// <returns>A string expression containing the inputted formula without whitespace.</returns>
        public override string ToString()
        {
            string expression = "";
            foreach (string token in tokens)
            {
                expression += token;
            }
            return expression;
        }

        /// <summary>
        /// We are using non-nullable object. Reports whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        /// <param name="obj">The object or formula that we are testing equality against the current formula</param>
        /// <returns>A boolean representing if two formulas or objects are equal.</returns>
        public override bool Equals(object? obj)
        {
            // must check if obj is null since overridden Equals method can take in a null object
            if (obj != null && obj.GetType().Equals(this.GetType())) // both have to be formulas
            {
                if (this.GetHashCode() != obj.GetHashCode()) return false;
                // if same hashcode then each entry of the normalized string must match exactly with the other.
                return this.ToString().Equals(obj.ToString());
            }
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// We are now using non-nullable objects
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// We are using non-nullable objects
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            foreach (string token in tokens)
            {
                hash.Add(token);
            }
            return hash.ToHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left parenthesis;
        /// right parenthesis; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal;and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        /// <param name="formula">a formula to be split apart into tokens</param>
        /// <returns>Yield returns an IEnumerable list which contains each separate token from the inputted expression.</returns>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";
            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                      lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);
            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {

        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason) : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}