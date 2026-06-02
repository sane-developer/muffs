namespace Muffs.Engine.AST.Generator;

public sealed record GeneratorOptions
{
    public Random Rng { get; set; }

    public DepthOptions Depth { get; set; }

    public ResultOptions Result { get; set; }
}

public readonly record struct DepthOptions(int Minimum, int Maximum)
{
    public static DepthOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }
}

public readonly record struct ResultOptions(int Minimum, int Maximum)
{
    public static ResultOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }
}
