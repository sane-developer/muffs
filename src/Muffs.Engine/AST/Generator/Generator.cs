using Muffs.Engine.AST.Expression;
using Muffs.Engine.AST.Generator.Cache;

namespace Muffs.Engine.AST.Generator;

public sealed class ExpressionGenerator(ExpressionGeneratorOptions options)
{
    private readonly CompositionRegistry _registry = CompositionRegistryGenerator.For(
        minimum: options.Operand.Minimum,
        maximum: options.Operand.Maximum
    );

    public Symbol Generate()
    {
        var length = options.Length.Random(options.Rng);

        var operands = new Stack<Symbol>(length);

        for (var i = 0; i < length; i++)
        {
            var value = options.Operand.Random(options.Rng);

            var depth = options.Depth.Random(options.Rng);

            var operand = Compose(value, depth);

            operands.Push(operand);
        }

        return Merge(operands);
    }

    private Symbol Compose(int value, int depth)
    {
        if (depth <= 0)
        {
            return Number.From(value);
        }

        var handler = GetRandomOperator();

        var compositions = _registry.Get(handler, value);

        if (compositions.IsEmpty)
        {
            return Number.From(value);
        }

        var position = options.Rng.Next(compositions.Length);

        var (lhs, rhs) = compositions[position];

        var left = Compose(lhs, depth - 1);

        var right = Compose(rhs, depth - 1);

        return Join(handler, left, right);
    }

    private Symbol Merge(Stack<Symbol> operands)
    {
        var expression = operands.Pop();

        while (operands.Count > 0)
        {
            var rhs = operands.Pop();

            var handler = GetSafeOperator();

            expression = Join(handler, rhs, expression);
        }

        return expression;
    }

    private static Symbol Join(Operator handler, Symbol lhs, Symbol rhs)
    {
        if (handler is Operator.Addition)
        {
            return Addition.From(lhs, rhs);
        }

        if (handler is Operator.Subtraction)
        {
            return Subtraction.From(lhs, rhs);
        }

        if (handler is Operator.Multiplication)
        {
            return Multiplication.From(lhs, rhs);
        }

        if (handler is Operator.Division)
        {
            return Division.From(lhs, rhs);
        }

        return Panic.UnknownOperator(handler);
    }


    private Operator GetSafeOperator()
    {
        var position = options.Rng.Next(_safeOperators.Length);

        return _safeOperators[position];
    }

    private Operator GetRandomOperator()
    {
        var position = options.Rng.Next(_operators.Length);

        return _operators[position];
    }

    private static readonly Operator[] _safeOperators =
    [
        Operator.Addition,
        Operator.Subtraction,
        Operator.Multiplication,
    ];

    private static readonly Operator[] _operators =
    [
        Operator.Addition,
        Operator.Subtraction,
        Operator.Multiplication,
        Operator.Division,
    ];
}

file static class Panic
{
    public static Symbol UnknownOperator(Operator handler)
    {
        throw new ArgumentOutOfRangeException($"Unknown operator: {handler}.");
    }
}
