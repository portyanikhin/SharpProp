using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SharpProp;

[ExcludeFromCodeCoverage]
internal static class CoolPropPInvoke
{
    static CoolPropPInvoke() => SwigExceptions.RegisterCallbacks();

#pragma warning disable CA2101
    [DllImport(Library.Name, EntryPoint = "CSharp_PropsSI")]
#pragma warning restore CA2101
    public static extern double PropsSI(string outputKey, string firstInputKey, double firstInputValue,
        string secondInputKey, double secondInputValue, string fluidName);

#pragma warning disable CA2101
    [DllImport(Library.Name, EntryPoint = "CSharp_HAPropsSI")]
#pragma warning restore CA2101
    public static extern double HAPropsSI(string outputKey, string firstInputKey, double firstInputValue,
        string secondInputKey, double secondInputValue, string thirdInputKey, double thirdInputValue);
}