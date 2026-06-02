using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST;

public static class Evaluator
{
    public static int Evaluate(this Symbol root)
    {
        return root switch
        {
            Number expression => expression.Value,
            Subtraction expression => expression.Subtract(),
            Multiplication expression => expression.Multiply(),
            Division expression => expression.Divide(),
            Addition expression => expression.Add(),
            _ => Panic.Unresolvable(root),
        };
    }
}

file static class Operator
{
    public static int Add(this Addition expression)
    {
        return expression.Lhs.Evaluate() + expression.Rhs.Evaluate();
    }

    public static int Subtract(this Subtraction expression)
    {
        return expression.Lhs.Evaluate() - expression.Rhs.Evaluate();
    }

    public static int Multiply(this Multiplication expression)
    {
        return expression.Lhs.Evaluate() * expression.Rhs.Evaluate();
    }

    public static int Divide(this Division expression)
    {
        return expression.Lhs.Evaluate() / expression.Rhs.Evaluate();
    }
}

file static class Panic
{
    public static int Unresolvable(this Symbol expression)
    {
        var type = expression.GetType().FullName;

        throw new NotImplementedException($"Panic: Provided unresolvable expression type: {type}.");
    }
}
