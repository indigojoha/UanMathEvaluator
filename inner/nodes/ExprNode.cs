using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
    internal abstract class ExprNode
    {
        public abstract double Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions);
    }
}
