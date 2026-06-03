namespace Muffs.Engine.AST.Generator;

public sealed record ExpressionGeneratorOptions
{
    public required Random Rng { get; init; }

    public required DepthOptions Depth { get; init; }

    public required LengthOptions Length { get; init; }

    public required ResultOptions Result { get; init; }

    public required OperandsOptions Operands { get; init; }
}

public sealed record DepthOptions(int Minimum, int Maximum)
{
    public static DepthOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }

    public int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}

public sealed record LengthOptions(int Minimum, int Maximum)
{
    public static LengthOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }

    public int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}

public sealed record OperandsOptions(int Minimum, int Maximum)
{
    public static OperandsOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }

    public int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}

public sealed record ResultOptions(int Minimum, int Maximum)
{
    public static ResultOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }

    public int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}
