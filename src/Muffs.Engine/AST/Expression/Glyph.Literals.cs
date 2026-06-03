namespace Muffs.Engine.AST.Expression;

public sealed record Number(int Value) : Glyph()
{
    public static Number From(int value)
    {
        return new(value);
    }
}
