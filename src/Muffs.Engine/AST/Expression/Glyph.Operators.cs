namespace Muffs.Engine.AST.Expression;

public sealed record Addition(Glyph Lhs, Glyph Rhs) : Glyph()
{
    public static Addition From(Glyph lhs, Glyph rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Subtraction(Glyph Lhs, Glyph Rhs) : Glyph()
{
    public static Subtraction From(Glyph lhs, Glyph rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Multiplication(Glyph Lhs, Glyph Rhs) : Glyph()
{
    public static Multiplication From(Glyph lhs, Glyph rhs)
    {
        return new(lhs, rhs);
    }
}

public sealed record Division(Glyph Lhs, Glyph Rhs) : Glyph()
{
    public static Division From(Glyph lhs, Glyph rhs)
    {
        return new(lhs, rhs);
    }
}
