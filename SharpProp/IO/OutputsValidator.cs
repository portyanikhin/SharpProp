namespace SharpProp;

/// <summary>
///     CoolProp outputs validator.
/// </summary>
internal class OutputsValidator
{
    private readonly double _output;

    /// <summary>
    ///     CoolProp outputs validator.
    /// </summary>
    /// <param name="output">CoolProp output.</param>
    public OutputsValidator(double output) => _output = output;

    /// <summary>
    ///     Validates the CoolProp output.
    /// </summary>
    /// <exception cref="ArgumentException">Invalid or not defined state!</exception>
    public void Validate()
    {
        if (double.IsInfinity(_output) || double.IsNaN(_output))
            throw new ArgumentException("Invalid or not defined state!");
    }
}