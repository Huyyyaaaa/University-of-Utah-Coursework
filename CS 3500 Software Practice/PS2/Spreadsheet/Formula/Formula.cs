// Skeleton written by Joe Zachary for CS 3500, January 2017

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        
        private List<string> formulaTokens;
        private List<string> verified;
        
        
        public Formula(String formula) :this(formula,  x=>x, x=>true)
        {
            
        }
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula, Normalizer n,  Validator v)
        {

           
            int openParen = 0;
            int closeParen = 0;

           
            if (formula == String.Empty || String.IsNullOrWhiteSpace(formula))
            {
                throw new FormulaFormatException("formula cannot be empty");
            }

            if(formula == null ||n == null || v == null)
            {
                throw new ArgumentNullException("cannot have null arguments");
            }
            

            if (formula.Length > 0)
            {
                formulaTokens = new List<string>(GetTokens(formula));
                verified = new List<string>();

                string firstT = formulaTokens[0];
                string lastT = formulaTokens[formulaTokens.Count - 1];

                

                if (!(firstT.Equals("(") || Double.TryParse(firstT, out double r) || Regex.IsMatch(firstT, @"^[a-zA-Z]+[\d]+$")))
                {
                    throw new FormulaFormatException("First character of formula must be open parentheses, number, or variable");
                }

                if (!(lastT.Equals(")")|| Double.TryParse(lastT, out double s) || Regex.IsMatch(lastT, @"^[a-zA-Z]+[\d]+$")))
                {
                    throw new FormulaFormatException("Last character of formula must be closing parentheses, number, or variable");
                }
            }
            else
            {
                throw new FormulaEvaluationException("Formula must be atleast one character in length.");
            }

            for (int i =0; i<formulaTokens.Count; i++)
            {
                if (formulaTokens[i].Equals("("))
                {
                    openParen++;
                }else if (formulaTokens[i].Equals(")"))
                {
                    closeParen++;
                }

                if (closeParen > openParen)
                {
                    throw new FormulaFormatException("Make sure your parentheses are correct. Too many closing parentheses found.");
                }

                if (i == 0)
                {
                    if(formulaTokens[i].Equals("(") || Double.TryParse(formulaTokens[i], out double r) || Regex.IsMatch(n(formulaTokens[i]), @"^[a-zA-Z]+[\d]+$"))
                    {
                        if(Regex.IsMatch(formulaTokens[i], @"^[a-zA-Z]+[\d]+$"))
                        {
                            string normalized = n(formulaTokens[i]);
                            if (!v(normalized))
                            {
                                throw new FormulaFormatException("Invalid variables in formula");
                            }
                            else
                            {
                                verified.Add(normalized);
                            }
                        }
                        else
                        {
                            verified.Add(formulaTokens[i]);
                        }
                    }
                    

                }else if((formulaTokens[i-1].Equals("(") || formulaTokens[i-1].Equals("+") || formulaTokens[i-1].Equals("-") || formulaTokens[i-1].Equals("*") || formulaTokens[i-1].Equals("/")  )
                    && (formulaTokens[i].Equals("(") || Double.TryParse(formulaTokens[i], out double result) || Regex.IsMatch(n(formulaTokens[i]), @"^[a-zA-Z]+[\d]+$")))
                {
                    if (Regex.IsMatch(n(formulaTokens[i]), @"[a-zA-Z][0-9a-zA-Z]*"))
                    {
                        string normalized = n(formulaTokens[i]);
                        if (!v(normalized))
                        {
                            throw new FormulaFormatException("Invalid variables in formula");
                        }
                        else
                        {
                            verified.Add(normalized);
                        }
                    }
                    else
                    {
                        verified.Add(formulaTokens[i]);
                    }


                } else if ((formulaTokens[i-1].Equals(")") || Double.TryParse(formulaTokens[i-1], out double res) || Regex.IsMatch(n(formulaTokens[i-1]), @"^[a-zA-Z]+[\d]+$")) 
                    && (formulaTokens[i].Equals("+") || formulaTokens[i].Equals("-") || formulaTokens[i].Equals("*") || formulaTokens[i].Equals("/") || formulaTokens[i].Equals(")")))
                {
                    
                    
                    
                        verified.Add(formulaTokens[i]);
                    
                }
                else
                {
                    throw new FormulaFormatException("Your formula format is incorrect");
                }
               
            }
            if(openParen != closeParen)
            {
                throw new FormulaFormatException("The number of opening parentheses is not equal to the number of closing parentheses");
            }

           
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            if(lookup == null)
            {
                throw new ArgumentNullException("lookup cannot be null");
            }
            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            //test for zero argument constructor and if it was called, behave like formula("0")
            if (verified == null)
            {
                verified = new List<string>();
                verified.Add("0");
            }

            for(int t = 0; t<verified.Count; t++)
            {
                if(Double.TryParse(verified[t], out double num))
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                       double first = values.Pop();
                        string op = operators.Pop();
                        double newVal;

                        if (op.Equals("*"))
                        {
                            newVal = first * num;
                            values.Push(newVal);
                        }
                        else
                        {
                            if (num == 0)
                            {
                                throw new FormulaEvaluationException("cannont divide by zero");
                            }
                            else
                            {
                                newVal = first / num;
                                values.Push(newVal);
                            }
                            
                        }
                    }
                    else
                    {
                        values.Push(num);
                    }
                }else if(Regex.IsMatch(verified[t], @"^[a-zA-Z]+[\d]+$"))
                {
                    double n;
                    try
                    {
                        n = lookup(verified[t]);

                    }catch(UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("undefined variable found in formula");
                    }
                    if (operators.Count>0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        double first = values.Pop();
                        string op = operators.Pop();
                        double newVal;

                        if (op.Equals("*"))
                        {
                            newVal = first * n;
                            values.Push(newVal);
                        }
                        else
                        {
                            if (n == 0) throw new FormulaEvaluationException("cannot divide by zero");
                            newVal = first / n;
                            values.Push(newVal);
                        }
                    }
                    else
                    {
                        values.Push(n);
                    }

                }

                if (verified[t].Equals("+") || verified[t].Equals("-"))
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
                    {
                        double x = values.Pop();
                        double y = values.Pop();
                        string z = operators.Pop();

                        if (z.Equals("+"))
                        {
                            values.Push(y + x);
                        }
                        else
                        {
                            values.Push(y - x);
                        }
                    }

                    operators.Push(verified[t]);
                }
                

                if (verified[t].Equals(")"))
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
                    {
                        double x = values.Pop();
                        double y = values.Pop();
                        string z = operators.Pop();

                        if (z.Equals("+"))
                        {
                            values.Push(y + x);
                        }
                        else
                        {
                            values.Push(y - x);
                        }
                    }

                    operators.Pop();

                    if(operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        double r = values.Pop();
                        double s = values.Pop();
                        string q = operators.Pop();

                        if (q.Equals("*"))
                        {
                            values.Push(r * s);
                        }
                        else
                        {
                            if (r == 0)
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                            else
                            {
                                values.Push(s / r);
                            }
                            
                        }
                    }
                }

                if(verified[t].Equals("*") || verified[t].Equals("/") || verified[t].Equals("("))
                {
                    operators.Push(formulaTokens[t]);
                }
            }

            if(operators.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                string oper = operators.Pop();
                double x = values.Pop();
                double y = values.Pop();
                if (oper.Equals("+"))
                {
                    return y + x;
                }
                else if(oper.Equals("-"))
                {
                    return y - x;
                }else if (oper.Equals("/") && x !=0 )
                {
                    return y / x;
                }
                else if(oper.Equals("*"))
                {
                    return y * x;
                }
                else
                {
                    throw new FormulaFormatException("cannot divide by zero");
                }
            }
            
        }



        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
      
        /// <summary>
        /// returns all variables i formula as an ISet<string>
        /// </summary>
        /// <returns></returns>
        public ISet<string> GetVariables()
        {
            HashSet<string> v = new HashSet<string>();

            //test for zero argument constructor and if it was called, behave like formula("0")
            if (verified == null)
            {
                verified = new List<string>();
                verified.Add("0");
            }
            foreach (string s in verified)
            {
                if(Regex.IsMatch(s, @"^[a-zA-Z]+[\d]+$"))
                {
                    v.Add(s);
                }
            }
            return v;
        }
        /// <summary>
        /// string representation of verified formula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //test for zero argument constructor and if it was called, behave like formula("0")
            if (verified == null)
            {
                verified = new List<string>();
                verified.Add("0");
            }
            string f = "";
            foreach(string s in verified)
            {
                f += s;
            }
            return f;
            
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);
    public delegate string Normalizer(string s);
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
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
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
