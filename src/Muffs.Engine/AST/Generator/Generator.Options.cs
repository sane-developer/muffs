namespace Muffs.Engine.AST.Generator;

public sealed record ExpressionGeneratorOptions
{
    public required Random Rng { get; init; }

    public required DepthOptions Depth { get; init; }

    public required LengthOptions Length { get; init; }

    public required OperandOptions Operand { get; init; }
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

public sealed record OperandOptions(int Minimum, int Maximum)
{
    public static OperandOptions Create(int minimum, int maximum)
    {
        return new(minimum, maximum);
    }

    public int Random(Random rng)
    {
        return rng.Next(Minimum, Maximum + 1);
    }
}
