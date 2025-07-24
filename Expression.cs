using UanMathEvaluator.inner;
using System.Collections.Generic;

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

        public MathVar Evaluate()
        {
            return Evaluate(Variables, Functions);
        }

        public MathVar Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            MathVar result = new MathVar();
            foreach (var statement in statements)
                result = statement.Evaluate(vars, functions);
            return result;
        }

        public override string ToString()
        {
            return string.Join("; ", statements);
        }
    }
}
