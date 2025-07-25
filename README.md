# UanMathEvaluator

## What is it?

**UanMathEvaluator** is a simple (not necessarily lightweight) C# library targeting **.NET Standard 2.0** that enables evaluation of multi-line mathematical expressions with support for function calls, variable assignments, and control flow like `if`, `for`, and `while`.

---

## How do I use it?

After referencing the DLL in your project, you can create and evaluate expressions like so:

```csharp
UanMathEvaluator.Expression expr = new UanMathEvaluator.Expression("x + y");
```

Assign values to variables (type `double`):

```csharp
expr.Variables["x"] = 1;
expr.Variables["y"] = 2;
```

Define custom functions that return a `double`:

```csharp
expr.Functions["halve"] = new UanMathEvaluator.MathMethod()
{
    ArgumentCount = 1,
    Method = args => args[0].Value * 0.5d
};

expr.Functions["while"] = new UanMathEvaluator.MathMethod()
{
    ArgumentCount = 2,
    Method = args =>
    {
        // evaluates the first argument as a body type 
        while (args[0].Expression.Evaluate(vars, functions).Value != 0)
        {
            evalArgs[1].Expression.Evaluate(vars, functions);
        }
        return 1;
    }
};
```

Then evaluate the expression:

```csharp
double result = expr.Evaluate(); // Returns 3
```

> ⚠️ **Note:** The parser strips all whitespace from expressions, which may affect variable or function names if spacing is relied on.

---

## What does the syntax look like?

### Argument Types

Functions take one of two argument types:

* **`value`** — an expression that's immediately evaluated during parsing
* **`body`** — a block of code inside `{}` that’s only evaluated when called

### General Syntax

You can use standard math expressions like:

```
x + y * sin(z)
```

You can also define variables, execute conditional logic, and write multi-line code:

```
x = 1;
y = 2;
if (x == 1,
  {
    z = 32
  },
  {
    z = 128
  });
z;
```

In this case, `Evaluate()` returns the value of `z`, as the final line determines the return value of the entire expression. It's good practice to explicitly return a final variable or result.

---

## Operators

The evaluator supports the following:

* **Arithmetic operators**: `+`, `-`, `*`, `/`, `^`, `%`
* **Logical operators**: `&&`, `||`
* **Comparison operators**: `==`, `!=`, `<`, `<=`, `>`, `>=`

---

## `if` Function

Conditional blocks are written as:

```
if (condition : value, trueBlock : body, falseBlock : body);
```

Internally, any non-zero value is considered **true**; `0` is **false**.

Example:

```
if (x > 0,
  { y = 1 },
  { y = -1 });
```

---

## `for` Function

Loop blocks look like this:

```
for (iterations : value, block : body)
```

You can access the current iteration index using the variable `iter`.

Nested loops are supported. Each nested level uses `iter1`, `iter2`, `iter3`, and so on:

```
for (3, {
  for (2, {
    total = total + iter * iter1;
  });
});
```

---

## `while` Function

While loops are defined like this:

```
while (condition : body, block : body)
```

The `block` runs repeatedly until `condition` evaluates to `0`. As with `if`, any non-zero value means true.

Example:

```
x = 5;
while ({ x > 0 }, {
  x = x - 1;
});
x;
```

This returns `0`.

---

Let me know if you want this formatted as markdown for a GitHub README or extended with advanced examples or internal implementation notes.
