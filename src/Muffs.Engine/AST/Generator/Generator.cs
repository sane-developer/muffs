using Muffs.Engine.AST.Expression;
using Muffs.Engine.AST.Generator.Cache;

namespace Muffs.Engine.AST.Generator;

public sealed class ExpressionGenerator(ExpressionGeneratorOptions options)
{
    private readonly CompositionIndex _index = CompositionIndexFactory.For(options.Operands, options.Result);

    public Glyph Generate()
    {
        var length = options.Length.Random(options.Rng);

        var operands = new Stack<Glyph>(length);

        for (var i = 0; i < length; i++)
        {
            var value = options.Operands.Random(options.Rng);

            var depth = options.Depth.Random(options.Rng);

            var operand = Compose(value, depth);

            operands.Push(operand);
        }

        return Merge(operands);
    }

    private Glyph Compose(int value, int depth)
    {
        if (depth <= 0)
        {
            return Number.From(value);
        }

        var function = GetRandomOperator();

        var compositions = _index.Get(function, value);

        if (compositions.IsEmpty())
        {
            return Number.From(value);
        }

        var composition = compositions.Random(options.Rng);

        var left = Compose(composition.Lhs, depth - 1);

        var right = Compose(composition.Rhs, depth - 1);

        return Join(function, left, right);
    }

    private Glyph Merge(Stack<Glyph> operands)
    {
        var expression = operands.Pop();

        while (operands.Count > 0)
        {
            var rhs = operands.Pop();

            var function = GetSafeOperator();

            expression = Join(function, rhs, expression);
        }

        return expression;
    }

    private static Glyph Join(Glyph.Operator function, Glyph lhs, Glyph rhs)
    {
        if (function is Glyph.Operator.Addition)
        {
            return Addition.From(lhs, rhs);
        }

        if (function is Glyph.Operator.Subtraction)
        {
            return Subtraction.From(lhs, rhs);
        }

        if (function is Glyph.Operator.Multiplication)
        {
            return Multiplication.From(lhs, rhs);
        }

        if (function is Glyph.Operator.Division)
        {
            return Division.From(lhs, rhs);
        }

        return Panic.UnknownOperator(function);
    }

    private Glyph.Operator GetSafeOperator()
    {
        var position = options.Rng.Next(_safeOperators.Length);

        return _safeOperators[position];
    }

    private Glyph.Operator GetRandomOperator()
    {
        var position = options.Rng.Next(_operators.Length);

        return _operators[position];
    }

    private static readonly Glyph.Operator[] _safeOperators =
    [
        Glyph.Operator.Addition,
        Glyph.Operator.Subtraction,
        Glyph.Operator.Multiplication,
    ];

    private static readonly Glyph.Operator[] _operators =
    [
        Glyph.Operator.Addition,
        Glyph.Operator.Subtraction,
        Glyph.Operator.Multiplication,
        Glyph.Operator.Division,
    ];
}

file static class Panic
{
    public static Glyph UnknownOperator(Glyph.Operator function)
    {
        throw new ArgumentOutOfRangeException($"Unknown operator: {function}.");
    }
}
