namespace SharpProp;

internal static class OutputsValidator
{
    public static void Validate(double output)
    {
        if (double.IsInfinity(output) || double.IsNaN(output))
        {
            throw new ArgumentException("Invalid or not defined state!");
        }
    }
}
