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

    [DllImport(Library.Name, EntryPoint = "CSharp_get_global_param_string")]
    public static extern string GetGlobalParamString(string paramName);

    [DllImport(Library.Name, EntryPoint = "CSharp_get_fluid_param_string")]
    public static extern string GetFluidParamString(string fluidName, string paramName);
}
