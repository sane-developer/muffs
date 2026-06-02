namespace Muffs.Engine.Session;

public sealed record Player(string Nickname)
{
    public static Player From(string nickname)
    {
        return new Player(nickname);
    }
}
