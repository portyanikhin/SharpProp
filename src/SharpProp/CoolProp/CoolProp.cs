// ReSharper disable UnusedMember.Global

namespace SharpProp;

[ExcludeFromCodeCoverage]
public static class CoolProp
{
    private static readonly object LibraryLock = new();

    static CoolProp() => SwigStrings.RegisterStringCallback();

    public static double PropsSI(
        string outputKey,
        string firstInputKey,
        double firstInputValue,
        string secondInputKey,
        double secondInputValue,
        string fluidName
    )
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.PropsSI(
                outputKey,
                firstInputKey,
                firstInputValue,
                secondInputKey,
                secondInputValue,
                fluidName
            );
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }

    public static double HAPropsSI(
        string outputKey,
        string firstInputKey,
        double firstInputValue,
        string secondInputKey,
        double secondInputValue,
        string thirdInputKey,
        double thirdInputValue
    )
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.HAPropsSI(
                outputKey,
                firstInputKey,
                firstInputValue,
                secondInputKey,
                secondInputValue,
                thirdInputKey,
                thirdInputValue
            );
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }

    public static string GetGlobalParamString(string paramName)
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.GetGlobalParamString(paramName);
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }

    public static string GetFluidParamString(string fluidName, string paramName)
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.GetFluidParamString(fluidName, paramName);
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }
}
