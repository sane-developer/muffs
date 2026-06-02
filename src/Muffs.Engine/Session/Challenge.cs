namespace Muffs.Engine.Session;

public sealed record Challenge(string Expression)
{
    public static Challenge From(string expression)
    {
        return new Challenge(expression);
    }
}
