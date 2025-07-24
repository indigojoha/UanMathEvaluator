using System;

namespace UanMathEvaluator.inner
{
    public struct MathMethod
    {
        public int ArgumentCount;
        public Func<double[], double> Method;
    }
}
