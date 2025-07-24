using UanMathEvaluator.inner;
using System;
using System.Collections.Generic;
using UanMathEvaluator.inner.nodes;

namespace UanMathEvaluator
{
    public class Expression
    {
        private readonly Parser parser;
        private readonly List<ExprNode> statements;
        public Dictionary<string, double> Variables { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, MathMethod> Functions { get; set; } = new Dictionary<string, MathMethod>();

        public Expression(string expresion)
        {
            parser = new Parser(expresion);
            statements = parser.ParseStatements();
        }

        public double Evaluate()
        {
            double result = 0;
            foreach (var statement in statements)
                result = statement.Evaluate(Variables, Functions);
            return result;
        }
    }
}
