using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
    internal class VariableNode : ExprNode
    {
        public readonly string Name;
        public VariableNode(string name) => Name = name;

        public override double Evaluate(Dictionary<string, double> variables, Dictionary<string, MathMethod> functions)
        {
            if (variables.TryGetValue(Name, out var value))
                return value;
            throw new KeyNotFoundException($"Undefined variable: {Name}");
        }
    }
}
