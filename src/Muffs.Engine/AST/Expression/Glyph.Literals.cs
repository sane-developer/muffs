namespace Muffs.Engine.AST.Expression;

public sealed record Number(int Value) : Symbol()
{
    public static Number From(int value)
    {
        return new(value);
    }
}
