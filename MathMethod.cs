using System;

namespace UanMathEvaluator
{
    public struct MathMethod
    {
        public int ArgumentCount;
        public Func<MathVar[], double> Method;
    }

    public enum MathMethodArgType
    {
        Value,
        Expression
    }

    public struct MathVar
    {
        public MathMethodArgType Type;
        public double Value;
        public Expression Expression;

        public MathVar(double value)
        {
            Type = MathMethodArgType.Value;
            Value = value;
            Expression = null;
        }

        public MathVar(Expression expression)
        {
            Type = MathMethodArgType.Expression;
            Value = -1;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression), "Expression cannot be null");
        }

        public override string ToString()
        {
            string result = "";
            if (Type == MathMethodArgType.Value)
            {
                result = "(Value)" + Value.ToString();
            }
            else if (Type == MathMethodArgType.Expression)
            {
                result = "(Expression)" + Expression?.ToString() ?? "null";
            }
            return result;
        }
    }
}
