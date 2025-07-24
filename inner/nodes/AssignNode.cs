using System;
using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
    internal class AssignNode : ExprNode
    {
        public readonly string Name;
        public readonly ExprNode Expr;

        public AssignNode(string name, ExprNode expr)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Variable name cannot be null");
            Expr = expr ?? throw new ArgumentNullException(nameof(expr), "Expression cannot be null");
        }

        public override double Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            double value = Expr.Evaluate(vars, functions);
            vars[Name] = value;
            return value;
        }
    }
}
