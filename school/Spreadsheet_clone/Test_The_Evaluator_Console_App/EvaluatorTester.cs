// See https://aka.ms/new-console-template for more information

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
/// This file runs tests on the Evaluator program.
/// 
/// </summary>

using FormulaEvaluator;

try
{
    Evaluator.Evaluate(null, null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing expression");
}

if (Evaluator.Evaluate("5 + 2", null) != 7)
{
    Console.WriteLine("Addition failure");
}

if (Evaluator.Evaluate("5 - 3", null) != 2)
{
    Console.WriteLine("Subtraction failure");
}

if (Evaluator.Evaluate("5 * 5", null) != 25)
{
    Console.WriteLine("Multiplication failure");
}

if (Evaluator.Evaluate("25 / 5", null) != 5)
{
    Console.WriteLine("Division failure");
}

if (Evaluator.Evaluate("5 + (5 * 6)", null) != 35)
{
    Console.WriteLine("Parenthesis failure");
}

if (Evaluator.Evaluate("5 * (3 + 2)", null) != 25)
{
    Console.WriteLine("Parenthesis 2 failure");
}

if (Evaluator.Evaluate("5+5*2", null) != 15)
{
    Console.WriteLine("Order of ops failure");
}

try
{
    Evaluator.Evaluate("5 / 0", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught divide by zero");
}

try
{
    Evaluator.Evaluate("5 * ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing rear number (mult)");
}

try
{
    Evaluator.Evaluate(" * 6", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing front number (mult)");
}

try
{
    Evaluator.Evaluate(" + + ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing number (+-)");
}

try
{
    Evaluator.Evaluate(" + ) ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing number (parenthesis)");
}

try
{
    Evaluator.Evaluate(" 5 + 6 ) * 3 ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing opening parenthesis");
}

try
{
    Evaluator.Evaluate(" ( 5 + 6 * 3 ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing closing parenthesis");
}

try
{
    Evaluator.Evaluate(" * (5 + 5) ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing number (parenthesis and mult)");
}

try
{
    Evaluator.Evaluate("5 / (5 - 5)", null);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught division by zero (parenthesis)");
}


/// <summary>
/// A dummy method to pass to the evaluator. Parameters self descriptive. If more variables are going to be used,
/// modify to use dictionary. For our small use case, if/else is fine.
/// </summary>
int variables(String varName)
{
    if (varName.Equals("A6"))
    {
        return 2;
    }
    else if (varName.Equals("B2"))
    {
        return 0;
    }
    else
    {
        throw new ArgumentException("Variable has no value");
    }
}

if (Evaluator.Evaluate("5 + A6", variables) != 7)
{
    Console.WriteLine("Variable lookup failure");
}

try
{
    Evaluator.Evaluate("2 + B8", variables);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught variable has no value");
}

try
{
    Evaluator.Evaluate(" 5 / B2", variables);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught variable has value of 0 when dividing");
}

try
{
    Evaluator.Evaluate("A6 +", variables);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught missing argument");
}

try
{
    Evaluator.Evaluate("5 + A", variables);
}
catch (ArgumentException)
{
    Console.WriteLine("Caught incomplete variable");
}