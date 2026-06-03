using System.Collections.Frozen;
using System.Collections.Immutable;
using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator;

internal sealed class CompositionRegistry(FrozenDictionary<CompositionKey, ImmutableArray<CompositionOperands>> entries)
{
    public ImmutableArray<CompositionOperands> Get(Operator handler, int result)
    {
        var key = CompositionKey.Create(handler, result);

        return entries.TryGetValue(key, out var compositions) ? compositions : [];
    }
}

internal sealed class CompositionRegistryGenerator
{
    public static CompositionRegistry For(int minimum, int maximum)
    {
        var compositions = Enumerate(minimum, maximum);

        var filtered = Filter(compositions, minimum, maximum);

        var groups = Group(filtered);

        var entries = groups.ToFrozenDictionary(
            grouping => grouping.Key,
            grouping => Map(grouping).ToImmutableArray()
        );

        return new CompositionRegistry(entries);
    }

    private static IEnumerable<CompositionOperands> Map(IGrouping<CompositionKey, Composition> grouping)
    {
        return grouping.Select(composition => CompositionOperands.Create(composition.Lhs, composition.Rhs));
    }

    private static IEnumerable<IGrouping<CompositionKey, Composition>> Group(IEnumerable<Composition> filtered)
    {
        return filtered.GroupBy(composition => CompositionKey.Create(composition.Handler, composition.Result));
    }

    private static IEnumerable<Composition> Filter(IEnumerable<Composition> compositions, int minimum, int maximum)
    {
        return compositions.Where(composition => composition.Result >= minimum && composition.Result <= maximum);
    }

    private static IEnumerable<Composition> Enumerate(int minimum, int maximum)
    {
        for (var lhs = minimum; lhs <= maximum; lhs++)
        {
            for (var rhs = minimum; rhs <= maximum; rhs++)
            {
                yield return Composition.Addition(lhs, rhs);

                yield return Composition.Subtraction(lhs, rhs);

                yield return Composition.Multiplication(lhs, rhs);

                if (IsPerfectDivisor(lhs, rhs))
                {
                    yield return Composition.Division(lhs, rhs);
                }
            }
        }
    }

    private static bool IsPerfectDivisor(int lhs, int rhs)
    {
        return rhs != 0 && lhs % rhs == 0;
    }
}

internal readonly record struct Composition(Operator Handler, int Lhs, int Rhs, int Result)
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

internal readonly record struct CompositionKey(Operator Handler, int Result)
{
    public static CompositionKey Create(Operator handler, int result)
    {
        return new(handler, result);
    }
}
