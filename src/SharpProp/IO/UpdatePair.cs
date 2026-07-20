namespace SharpProp;

internal record UpdatePair(input_pairs InputPair, double FirstValue, double SecondValue)
{
    public input_pairs InputPair { get; } = InputPair;
    public double FirstValue { get; } = FirstValue;
    public double SecondValue { get; } = SecondValue;
}
