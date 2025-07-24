using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
    internal class NumberNode : ExprNode
    {
        public readonly double Value;
        public NumberNode(double value) => Value = value;
        public override double Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions) => Value;
    }
}
