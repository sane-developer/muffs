namespace Muffs.Engine.AST.Expression;

public abstract record Symbol
{
    public enum Operator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
    }
}
