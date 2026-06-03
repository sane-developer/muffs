using System.Collections.Frozen;
using System.Collections.Immutable;
using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator.Cache;

internal sealed class CompositionRegistry(FrozenDictionary<CompositionKey, ImmutableArray<CompositionOperands>> entries)
{
    public ImmutableArray<CompositionOperands> Get(Operator function, int result)
    {
        var key = CompositionKey.Create(function, result);

        return entries.TryGetValue(key, out var compositions) ? compositions : [];
    }
}
