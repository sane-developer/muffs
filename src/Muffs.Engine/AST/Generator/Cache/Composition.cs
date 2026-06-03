using System.Collections.Immutable;
using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator.Cache;

internal readonly record struct Composition(Glyph.Operator Function, int Lhs, int Rhs, int Result)
{
    public static Composition Addition(int lhs, int rhs)
    {
        return new Composition(Glyph.Operator.Addition, lhs, rhs, lhs + rhs);
    }

    public static Composition Subtraction(int lhs, int rhs)
    {
        return new Composition(Glyph.Operator.Subtraction, lhs, rhs, lhs - rhs);
    }

    public static Composition Multiplication(int lhs, int rhs)
    {
        return new Composition(Glyph.Operator.Multiplication, lhs, rhs, lhs * rhs);
    }

    public static Composition Division(int lhs, int rhs)
    {
        return new Composition(Glyph.Operator.Division, lhs, rhs, lhs / rhs);
    }
}

internal readonly record struct CompositionOperands(int Lhs, int Rhs)
{
    public static CompositionOperands Create(int lhs, int rhs)
    {
        return new(lhs, rhs);
    }
}

internal readonly record struct CompositionKey(Glyph.Operator Function, int Result)
{
    public static CompositionKey Create(Glyph.Operator function, int result)
    {
        return new(function, result);
    }
}

internal readonly record struct CompositionSet(ImmutableArray<CompositionOperands> Compositions)
{
    public static CompositionSet Create(IEnumerable<CompositionOperands> compositions)
    {
        return new([.. compositions]);
    }

    public CompositionOperands Random(Random rng)
    {
        var index = rng.Next(Compositions.Length);

        return Compositions[index];
    }

    public bool IsEmpty()
    {
        return Compositions.IsDefaultOrEmpty;
    }
}
