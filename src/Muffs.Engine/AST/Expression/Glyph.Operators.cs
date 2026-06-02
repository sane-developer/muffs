namespace Muffs.Engine.AST.Expression;

public sealed record Addition(Symbol Lhs, Symbol Rhs) : Symbol()
{
    public static Addition From(Symbol lhs, Symbol rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Subtraction(Symbol Lhs, Symbol Rhs) : Symbol()
{
    public static Subtraction From(Symbol lhs, Symbol rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Multiplication(Symbol Lhs, Symbol Rhs) : Symbol()
{
    public static Multiplication From(Symbol lhs, Symbol rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Division(Symbol Lhs, Symbol Rhs) : Symbol()
{
    public static Division From(Symbol lhs, Symbol rhs)
    {
        return new(lhs, rhs);
    }
}

public enum Operator
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}
