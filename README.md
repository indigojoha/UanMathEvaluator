# UanMathEvaluator
## What is it?
**UanMathEvaluator** is a simple netstandard2.0 library written in C# that adds the ability to evaluate multi-line mathematical expressions with function calls, variable assignments, and conditionals.  
## How do I use it?
Once you're referincing the DLL in your project, simply create an expression object:
```C#
UanMathEvaluator.Expression expr = new UanMathEvaluator.Expression("x + y");
```
Then you can set variables of type `double`:
```C#
expr.Variables["x"] = 1;
expr.Variables["y"] = 2;
```
or functions that return `double`:
```C#
expr.Functions["halve"] = new UanMathEvaluator.inner.MathMethod()
{
  ArgumentCount = 1,
  Method = args => args[0] * 0.5d
};
```
Finally, you can evaluate it with:
```C#
double result = expr.Evaluate(); // this returns 3 
```
Please note that variables and functions will not work as the parser completely removes all whitespaces.  
## What do the expressions look like?
It supports normal linear math expressions like `x + y * sin(z)`.
However, it also supports multiple expressions with variable assignment and if blocks:
```
x = 1;
y = 2;
if (x == 1,
  z = 32,
  z = 128);
z;
```
This will cause `z` to be the result of `Evaluate()`, otherwise the result would be the result of the `if` block, which is still `z` in this example, but it's good practice to make the returned variable explicit.  
`if` blocks are structured like so:
```
if (condition expression, true expression, false expression);
```
