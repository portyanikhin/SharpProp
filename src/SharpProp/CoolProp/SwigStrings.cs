namespace SharpProp;

[ExcludeFromCodeCoverage]
internal static class SwigStrings
{
    private delegate string SwigStringDelegate(string message);
    private static readonly SwigStringDelegate StringDelegate = CreateString;

    public static void RegisterStringCallback() =>
        SWIGRegisterStringCallback_CoolProp(StringDelegate);

    [DllImport(Library.Name, EntryPoint = "SWIGRegisterStringCallback_CoolProp")]
    private static extern void SWIGRegisterStringCallback_CoolProp(
        SwigStringDelegate stringDelegate
    );

    private static string CreateString(string cString) => cString;
}
