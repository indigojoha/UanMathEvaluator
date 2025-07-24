using System;
using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
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

        public override double Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            double l = Left.Evaluate(vars, functions);
            double r = Right.Evaluate(vars, functions);

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

            return result;
        }
    }
}
