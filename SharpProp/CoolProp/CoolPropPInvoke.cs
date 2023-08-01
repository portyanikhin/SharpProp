namespace SharpProp;

[ExcludeFromCodeCoverage]
internal static class CoolPropPInvoke
{
    [DllImport(Library.Name, EntryPoint = "CSharp_PropsSI")]
    public static extern double PropsSI(
        string outputKey,
        string firstInputKey,
        double firstInputValue,
        string secondInputKey,
        double secondInputValue,
        string fluidName
    );

    [DllImport(Library.Name, EntryPoint = "CSharp_HAPropsSI")]
    public static extern double HAPropsSI(
        string outputKey,
        string firstInputKey,
        double firstInputValue,
        string secondInputKey,
        double secondInputValue,
        string thirdInputKey,
        double thirdInputValue
    );
}
