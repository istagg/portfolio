using System.Text.RegularExpressions;

namespace FormulaEvaluator {
    
    /// <summary>
    /// Author:     Isaac Stagg
    /// Parter:     None
    /// Date:       1/12/2022
    /// Course:     CS 3500, University of Utah, School of Computing
    /// Copyright:  CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework
    /// 
    /// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source. All references used in the completion of the assignment are cited in my README file.
    /// 
    /// File Contents:
    /// This file calculates the value of an expression passed in as a string. This program removes white space
    /// and checks for errors such as dividing by zero or missing an argument.
    /// 
    /// </summary>

    public class Evaluator
    {
        public delegate int Lookup(String variable_name);

        /// <summary>
        /// Functions as name indicates.
        /// </summary>
        /// <exception cref="ArgumentException"> Throws when divide by zero occurs. </exception>
        private static int ComputeGivenOperation(String op, int val1, int val2)
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
            else if (op.Equals("/"))
            {
                if (val1 != 0)
                {
                    return val2 / val1;
                }
                else
                {
                    throw new ArgumentException("Division by zero.");
                }
            }
            return 0;
        }

        /// <summary>
        /// This method evaluates a given formula and computes the value.
        /// Be aware that the expression must not include negative integers. Multiplication must be explicit,
        /// cannot imply it with an integer in front of parentheses.
        /// </summary>
        /// <param name="expression"> A string representation of the formula to be calculated. </param>
        /// <param name="variableEvaluator"> A function that finds the value of a variable given in the expression. </param>
        /// <returns> The value calculated from the given expression. </returns>
        /// <exception cref="ArgumentException"> Throws if the math is not possible. For example, divide by zero, missing operator, missing argument, etc.  </exception>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int> valueStack = new Stack<int>();
            Stack<String> operatorStack = new Stack<String>();

            string[] substrings = Regex.Split(expression, @"(\()|(\))|(-)|(\+)|(\*)|(/)");

            foreach (string token in substrings)
            {
                if (token.Equals("") || token.Equals(" "))
                {
                    continue;
                }

                int num = 0;
                if (int.TryParse(token, out num)) // checks if token is an integer. If it is, stored in num
                {
                    if (operatorStack.Count() == 0)
                    {
                        valueStack.Push(num);
                    }
                    else if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                    {
                        if (valueStack.Count() == 0)
                        {
                            throw new ArgumentException("Missing one or both numbers for operator.");
                        }
                        valueStack.Push(ComputeGivenOperation(operatorStack.Pop(), num, valueStack.Pop()));
                    }
                    else
                    {
                        valueStack.Push(num);
                    }
                }
                else if (token.Equals("+") || token.Equals("-"))
                {
                    if (operatorStack.Count() == 0)
                    {
                        operatorStack.Push(token);
                    }
                    else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        if (valueStack.Count() < 2)
                        {
                            throw new ArgumentException("Not enough numbers to add.");
                        }
                        int val1 = valueStack.Pop();
                        int val2 = valueStack.Pop();
                        valueStack.Push(ComputeGivenOperation(operatorStack.Pop(), val1, val2));
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
                    if (operatorStack.Peek().Equals("("))
                    {
                        operatorStack.Pop();
                    }
                    else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        if (valueStack.Count() < 2)
                        {
                            throw new ArgumentException("Not enough numbers to add");
                        }
                        int val1 = valueStack.Pop();
                        int val2 = valueStack.Pop();
                        String op = operatorStack.Pop();
                        if (op.Equals("+") || op.Equals("-"))
                        {
                            valueStack.Push(ComputeGivenOperation(op, val1, val2));
                        }
                        if (operatorStack.Count() > 0 && operatorStack.Peek().Equals("("))
                        {
                            operatorStack.Pop();
                        }
                        else
                        {
                            throw new ArgumentException("Expected '(' missing.");
                        }
                        if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            val1 = valueStack.Pop();
                            val2 = valueStack.Pop();
                            valueStack.Push(ComputeGivenOperation(operatorStack.Pop(), val1, val2));
                        }
                    }
                }
                else
                {
                    char[] characters = token.ToCharArray();
                    if (char.IsLetter(characters[0]))
                    {
                        for (int i = 1; i < characters.Length; i++)
                        {
                            if (!char.IsDigit(characters[i]))
                            {
                                throw new ArgumentException();
                            }
                        }
                    }
                    try
                    {
                        num = variableEvaluator(String.Concat(token.Where(c => !Char.IsWhiteSpace(c)))); // removes white space
                    }
                    catch (ArgumentException)
                    {
                        throw new ArgumentException("Variable has no value.");
                    }
                    if (operatorStack.Count() == 0)
                    {
                        valueStack.Push(num);
                    }
                    else if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                    {
                        if (valueStack.Count() == 0)
                        {
                            throw new ArgumentException("Missing one or both numbers for operator.");
                        }
                        valueStack.Push(ComputeGivenOperation(operatorStack.Pop(), num, valueStack.Pop()));
                    }
                    else
                    {
                        valueStack.Push(num);
                    }
                }
            }
            if (operatorStack.Count() == 0 && valueStack.Count() == 1)
            {
                return valueStack.Pop();
            }
            else
            {
                if (operatorStack.Count() > 1 || valueStack.Count() != 2)
                {
                    throw new ArgumentException("Too many operators or values.");
                }
                else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    return ComputeGivenOperation(operatorStack.Pop(), val1, val2);
                }
                else
                {
                    throw new ArgumentException("Failure");
                }
            }
        }
    }
}