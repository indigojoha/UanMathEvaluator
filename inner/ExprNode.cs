using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UanMathEvaluator.inner
{
    internal abstract class ExprNode
    {
        public abstract MathVar Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions);
    }

    internal class ExpressionNode : ExprNode
    {
        public readonly Expression Expr;

        public ExpressionNode(string expr)
        {
            Expr = new Expression(expr);
        }

        public override MathVar Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions)
        {
            return new MathVar(Expr);
        }
    }

    internal class BinaryOpNode : ExprNode
    {
        public readonly string Op;
        public readonly ExprNode Left, Right;

        public BinaryOpNode(string op, ExprNode left, ExprNode right)
        {
            Op = op;
            Left = left;
            Right = right;
        }

        public override MathVar Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            double l = Left.Evaluate(vars, functions).Value;
            double r = Right.Evaluate(vars, functions).Value;

            double result;

            switch (Op)
            {
                case "+":
                    result = l + r;
                    break;
                case "-":
                    result = l - r;
                    break;
                case "*":
                    result = l * r;
                    break;
                case "/":
                    if (r == 0) throw new DivideByZeroException("Division by zero is not allowed");
                    result = l / r;
                    break;
                case "%":
                    if (r == 0) throw new DivideByZeroException("Division by zero is not allowed");
                    result = l % r;
                    break;
                case "^":
                    result = Math.Pow(l, r);
                    break;
                case "==":
                    result = l == r ? 1 : 0;
                    break;
                case "!=":
                    result = l != r ? 1 : 0;
                    break;
                case "<":
                    result = l < r ? 1 : 0;
                    break;
                case "<=":
                    result = l <= r ? 1 : 0;
                    break;
                case ">":
                    result = l > r ? 1 : 0;
                    break;
                case ">=":
                    result = l >= r ? 1 : 0;
                    break;
                default:
                    throw new Exception($"Unknown operator {Op}");
            }

            return new MathVar(result);
        }
    }

    internal class AssignNode : ExprNode
    {
        public readonly string Name;
        public readonly ExprNode Expr;

        public AssignNode(string name, ExprNode expr)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Variable name cannot be null");
            Expr = expr ?? throw new ArgumentNullException(nameof(expr), "Expression cannot be null");
        }

        public override MathVar Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            double value = Expr.Evaluate(vars, functions).Value;
            vars[Name] = value;
            return new MathVar(value);
        }
    }

    internal class NumberNode : ExprNode
    {
        public readonly double Value;
        public NumberNode(double value) => Value = value;
        public override MathVar Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions) => new MathVar(Value);
    }

    internal class VariableNode : ExprNode
    {
        public readonly string Name;
        public VariableNode(string name) => Name = name;

        public override MathVar Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions)
        {
            if (variables.TryGetValue(Name, out var value))
                return (new MathVar(value));
            throw new KeyNotFoundException($"Undefined variable: {Name}");
        }
    }

    internal class FunctionNode : ExprNode
    {
        public readonly string Name;
        public readonly List<ExprNode> Args;

        public FunctionNode(string name, List<ExprNode> args)
        {
            Name = name;
            Args = args;
        }

        public override MathVar Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            var evalArgs = Args.ConvertAll(arg => arg.Evaluate(vars, functions));

            double result;

            int forDepth = 0;

            switch (Name.ToLower())
            {
                case "max":
                    result = Math.Max(evalArgs[0].Value, evalArgs[1].Value);
                    break;
                case "min":
                    result = Math.Min(evalArgs[0].Value, evalArgs[1].Value);
                    break;
                case "abs":
                    result = Math.Abs(evalArgs[0].Value);
                    break;
                case "sqrt":
                    result = Math.Sqrt(evalArgs[0].Value);
                    break;
                case "pow":
                    result = Math.Pow(evalArgs[0].Value, evalArgs[1].Value);
                    break;
                case "sin":
                    result = Math.Sin(evalArgs[0].Value);
                    break;
                case "cos":
                    result = Math.Cos(evalArgs[0].Value);
                    break;
                case "tan":
                    result = Math.Tan(evalArgs[0].Value);
                    break;
                case "log":
                    result = Math.Log(evalArgs[0].Value, evalArgs.Count > 1 ? evalArgs[1].Value : Math.E);
                    break;
                case "exp":
                    result = Math.Exp(evalArgs[0].Value);
                    break;
                case "round":
                    result = Math.Round(evalArgs[0].Value, evalArgs.Count > 1 ? (int)evalArgs[1].Value : 0);
                    break;
                case "ceil":
                case "ceiling":
                    result = Math.Ceiling(evalArgs[0].Value);
                    break;
                case "floor":
                    result = Math.Floor(evalArgs[0].Value);
                    break;
                case "sign":
                    result = Math.Sign(evalArgs[0].Value);
                    break;
                case "if":
                    if (evalArgs.Count != 3)
                        throw new ArgumentException("Function 'if' expects 3 arguments: condition value, true expression, false expression");
                    if (evalArgs[0].Value != 0)
                        result = evalArgs[1].Expression.Evaluate(vars, functions).Value;
                    else
                        result = evalArgs[2].Expression.Evaluate().Value;
                    break;
                case "for":
                    if (evalArgs.Count != 2)
                        throw new ArgumentException("Function 'for' expects 2 arguments: iteration cound and loop expression");
                    int iterations = (int)evalArgs[0].Value;
                    forDepth++;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (forDepth == 1)
                            vars["iter"] = i;
                        else
                            vars["iter" + forDepth] = i;
                        evalArgs[1].Expression.Evaluate(vars, functions);
                    }
                    forDepth--;
                    result = 1d;
                    break;
                case "while":
                    if (evalArgs.Count != 2)
                        throw new ArgumentException("Function 'while' expects 2 arguments: condition expression and loop expression");
                    while (evalArgs[0].Expression.Evaluate(vars, functions).Value != 0)
                    {
                        evalArgs[1].Expression.Evaluate(vars, functions);
                    }
                    result = 1d;
                    break;
                default:
                    if (functions.TryGetValue(Name, out var func))
                    {
                        if (evalArgs.Count != func.ArgumentCount)
                            throw new ArgumentException($"Function '{Name}' expects {func.ArgumentCount} arguments, but got {evalArgs.Count}.");
                        result = func.Method.Invoke(evalArgs.ToArray());
                        break;
                    }
                    throw new Exception($"Unknown function: '{Name}'");
            }

            return new MathVar(result);
        }
    }
}
