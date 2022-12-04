using System.Diagnostics.CodeAnalysis;

namespace SharpProp;

[ExcludeFromCodeCoverage]
public static class CoolProp
{
    private static readonly object LibraryLock = new();

    public static double PropsSI(string outputKey, string firstInputKey, double firstInputValue,
        string secondInputKey, double secondInputValue, string fluidName)
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.PropsSI(outputKey, firstInputKey, firstInputValue,
                secondInputKey, secondInputValue, fluidName);
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }

    public static double HAPropsSI(string outputKey, string firstInputKey, double firstInputValue,
        string secondInputKey, double secondInputValue, string thirdInputKey, double thirdInputValue)
    {
        lock (LibraryLock)
        {
            var result = CoolPropPInvoke.HAPropsSI(outputKey, firstInputKey, firstInputValue,
                secondInputKey, secondInputValue, thirdInputKey, thirdInputValue);
            SwigExceptions.ThrowPendingException();
            return result;
        }
    }
}