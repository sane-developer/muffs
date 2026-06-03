using System.Collections.Frozen;
using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator.Cache;

internal sealed class CompositionIndex(FrozenDictionary<CompositionKey, CompositionSet> entries)
{
    public CompositionSet Get(Symbol.Operator function, int result)
    {
        var key = CompositionKey.Create(function, result);

        return entries.TryGetValue(key, out var set) ? set : KnownCompositionSets.Empty;
    }
}

internal static class CompositionIndexFactory
{
    public static CompositionIndex For(OperandsOptions operands, ResultOptions result)
    {
        var combinations = Enumerate(operands.Minimum, operands.Maximum);

        var compositions = Filter(combinations, result.Minimum, result.Maximum);

        var groups = Group(compositions);

        var entries = Freeze(groups);

        return new CompositionIndex(entries);
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

                if (KnownCompositionRules.IsPerfectDivisor(lhs, rhs))
                {
                    yield return Composition.Division(lhs, rhs);
                }
            }
        }
    }

    private static IEnumerable<Composition> Filter(IEnumerable<Composition> compositions, int minimum, int maximum)
    {
        return compositions.Where(c => c.Result >= minimum && c.Result <= maximum);
    }

    private static IEnumerable<IGrouping<CompositionKey, Composition>> Group(IEnumerable<Composition> compositions)
    {
        return compositions.GroupBy(c => CompositionKey.Create(c.Function, c.Result));
    }

    private static IEnumerable<CompositionOperands> Map(IGrouping<CompositionKey, Composition> grouping)
    {
        return grouping.Select(c => CompositionOperands.Create(c.Lhs, c.Rhs));
    }

    private static FrozenDictionary<CompositionKey, CompositionSet> Freeze(IEnumerable<IGrouping<CompositionKey, Composition>> groups)
    {
        static CompositionKey key(IGrouping<CompositionKey, Composition> grouping)
        {
            return grouping.Key;
        }

        static CompositionSet value(IGrouping<CompositionKey, Composition> grouping)
        {
            var compositions = Map(grouping);

            return CompositionSet.Create(compositions);
        }

        return groups.ToFrozenDictionary(key, value);
    }
}

file static class KnownCompositionRules
{
    public static bool IsPerfectDivisor(int lhs, int rhs)
    {
        return rhs != 0 && lhs % rhs == 0;
    }
}

file static class KnownCompositionSets
{
    public static readonly CompositionSet Empty = CompositionSet.Create(compositions: []);
}
