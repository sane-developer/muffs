using Muffs.Engine.AST.Expression;

namespace Muffs.Engine.AST.Generator;

public sealed class Generator(GeneratorOptions options)
{
    public Symbol Generate()
    {
        return new Number(0);
    }
}
