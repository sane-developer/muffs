using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Muffs.Engine.AST.Generator.Cache;

internal sealed class CompositionRegistryGenerator
{
    public static CompositionRegistry For(int minimum, int maximum)
    {
        var source = Enumerate(minimum, maximum);

        var compositions = Filter(source, minimum, maximum);

        var groups = Group(compositions);

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
        return filtered.GroupBy(composition => CompositionKey.Create(composition.Function, composition.Result));
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
