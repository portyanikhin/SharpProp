namespace SharpProp;

internal record UpdatePair(
    InputPairs InputPair,
    double FirstValue,
    double SecondValue
)
{
    public InputPairs InputPair { get; } = InputPair;
    public double FirstValue { get; } = FirstValue;
    public double SecondValue { get; } = SecondValue;
}
