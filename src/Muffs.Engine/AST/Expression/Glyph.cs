namespace Muffs.Engine.AST.Expression;

public abstract record Glyph
{
    public enum Operator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
    }
}
