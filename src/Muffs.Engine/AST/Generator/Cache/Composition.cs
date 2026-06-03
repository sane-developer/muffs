using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator.Cache;

internal readonly record struct Composition(Operator Function, int Lhs, int Rhs, int Result)
{
    public static Composition Addition(int lhs, int rhs)
    {
        return new Composition(Operator.Addition, lhs, rhs, lhs + rhs);
    }

    public static Composition Subtraction(int lhs, int rhs)
    {
        return new Composition(Operator.Subtraction, lhs, rhs, lhs - rhs);
    }

    public static Composition Multiplication(int lhs, int rhs)
    {
        return new Composition(Operator.Multiplication, lhs, rhs, lhs * rhs);
    }

    public static Composition Division(int lhs, int rhs)
    {
        return new Composition(Operator.Division, lhs, rhs, lhs / rhs);
    }
}

internal readonly record struct CompositionOperands(int Lhs, int Rhs)
{
    public static CompositionOperands Create(int lhs, int rhs)
    {
        return new(lhs, rhs);
    }
}

internal readonly record struct CompositionKey(Operator Function, int Result)
{
    public static CompositionKey Create(Operator function, int result)
    {
        return new(function, result);
    }
}
