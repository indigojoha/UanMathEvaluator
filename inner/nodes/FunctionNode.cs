using System;
using System.Collections.Generic;

namespace UanMathEvaluator.inner.nodes
{
    internal class FunctionNode : ExprNode
    {
        public readonly string Name;
        public readonly List<ExprNode> Args;

        public FunctionNode(string name, List<ExprNode> args)
        {
            Name = name;
            Args = args;
        }

        public override double Evaluate(Dictionary<string, double> vars, Dictionary<string, MathMethod> functions)
        {
            var evalArgs = Args.ConvertAll(arg => arg.Evaluate(vars, functions));

            double result;

            switch (Name.ToLower())
            {
                case "max":
                    result = Math.Max(evalArgs[0], evalArgs[1]);
                    break;
                case "min":
                    result = Math.Min(evalArgs[0], evalArgs[1]);
                    break;
                case "abs":
                    result = Math.Abs(evalArgs[0]);
                    break;
                case "sqrt":
                    result = Math.Sqrt(evalArgs[0]);
                    break;
                case "pow":
                    result = Math.Pow(evalArgs[0], evalArgs[1]);
                    break;
                case "sin":
                    result = Math.Sin(evalArgs[0]);
                    break;
                case "cos":
                    result = Math.Cos(evalArgs[0]);
                    break;
                case "tan":
                    result = Math.Tan(evalArgs[0]);
                    break;
                case "log":
                    result = Math.Log(evalArgs[0], evalArgs.Count > 1 ? evalArgs[1] : Math.E);
                    break;
                case "exp":
                    result = Math.Exp(evalArgs[0]);
                    break;
                case "round":
                    result = Math.Round(evalArgs[0], evalArgs.Count > 1 ? (int)evalArgs[1] : 0);
                    break;
                case "ceil":
                case "ceiling":
                    result = Math.Ceiling(evalArgs[0]);
                    break;
                case "floor":
                    result = Math.Floor(evalArgs[0]);
                    break;
                case "sign":
                    result = Math.Sign(evalArgs[0]);
                    break;
                case "if":
                    result = evalArgs[0] != 0? evalArgs[1] : evalArgs[2];
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

            return result;
        }
    }
}
