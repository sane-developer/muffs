namespace Muffs.Engine.Session;

public sealed class Session(Player player)
{
    private int _score;

    private int _correct;

    private int _incorrect;

    public static Session Start(Player player)
    {
        return new Session(player);
    }
}
