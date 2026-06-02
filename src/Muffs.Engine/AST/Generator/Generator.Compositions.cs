using System.Collections.Frozen;
using System.Collections.Immutable;
using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator;

internal sealed class CompositionRegistry
{
    private readonly FrozenDictionary<CompositionKey, ImmutableArray<Operands>> _entries;

    private CompositionRegistry(FrozenDictionary<CompositionKey, ImmutableArray<Operands>> entries)
    {
        _entries = entries;
    }

    public static CompositionRegistry Build(int minimum, int maximum)
    {
        var entries = new Dictionary<CompositionKey, List<Operands>>();

        void Add(Operator op, int result, int lhs, int rhs)
        {
            if (result < minimum || result > maximum)
            {
                return;
            }

            var key = new CompositionKey(result, op);

            if (!entries.TryGetValue(key, out var bucket))
            {
                entries[key] = bucket = [];
            }

            bucket.Add(new Operands(lhs, rhs));
        }

        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                Add(Operator.Addition, lhs + rhs, lhs, rhs);
                Add(Operator.Subtraction, lhs - rhs, lhs, rhs);
                Add(Operator.Multiplication, lhs * rhs, lhs, rhs);

                if (rhs != 0 && lhs % rhs == 0)
                {
                    Add(Operator.Division, lhs / rhs, lhs, rhs);
                }
            }
        }

        var frozen = entries.ToFrozenDictionary(entry => entry.Key, entry => entry.Value.ToImmutableArray());

        return new CompositionRegistry(frozen);
    }

    public ImmutableArray<Operands> Get(int result, Operator op)
    {
        return _entries.TryGetValue(new CompositionKey(result, op), out var compositions)
            ? compositions
            : ImmutableArray<Operands>.Empty;
    }
}

internal readonly record struct CompositionKey(int Result, Operator Op);

internal readonly record struct Operands(int Lhs, int Rhs);
