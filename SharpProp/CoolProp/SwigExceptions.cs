namespace SharpProp;

[ExcludeFromCodeCoverage]
internal static class SwigExceptions
{
    [ThreadStatic]
    private static Exception? _pendingException;
    private static int _pendingExceptionsCount;
    private static readonly object ExceptionsLock = new();

    private static readonly ExceptionDelegate ApplicationDelegate =
        SetPendingApplicationException;

    private static readonly ExceptionDelegate ArithmeticDelegate =
        SetPendingArithmeticException;

    private static readonly ExceptionDelegate DivideByZeroDelegate =
        SetPendingDivideByZeroException;

    private static readonly ExceptionDelegate IndexOutOfRangeDelegate =
        SetPendingIndexOutOfRangeException;

    private static readonly ExceptionDelegate InvalidCastDelegate =
        SetPendingInvalidCastException;

    private static readonly ExceptionDelegate InvalidOperationDelegate =
        SetPendingInvalidOperationException;

    private static readonly ExceptionDelegate IoDelegate =
        SetPendingIoException;

    private static readonly ExceptionDelegate NullReferenceDelegate =
        SetPendingNullReferenceException;

    private static readonly ExceptionDelegate OutOfMemoryDelegate =
        SetPendingOutOfMemoryException;

    private static readonly ExceptionDelegate OverflowDelegate =
        SetPendingOverflowException;

    private static readonly ExceptionDelegate SystemDelegate =
        SetPendingSystemException;

    private static readonly ExceptionArgumentDelegate ArgumentDelegate =
        SetPendingArgumentException;

    private static readonly ExceptionArgumentDelegate ArgumentNullDelegate =
        SetPendingArgumentNullException;

    private static readonly ExceptionArgumentDelegate ArgumentOutOfRangeDelegate =
        SetPendingArgumentOutOfRangeException;

    static SwigExceptions()
    {
        RegisterExceptionCallbacks(
            ApplicationDelegate,
            ArithmeticDelegate,
            DivideByZeroDelegate,
            IndexOutOfRangeDelegate,
            InvalidCastDelegate,
            InvalidOperationDelegate,
            IoDelegate,
            NullReferenceDelegate,
            OutOfMemoryDelegate,
            OverflowDelegate,
            SystemDelegate
        );
        RegisterExceptionArgumentCallbacks(
            ArgumentDelegate,
            ArgumentNullDelegate,
            ArgumentOutOfRangeDelegate
        );
    }

    public static void ThrowPendingException()
    {
        if (_pendingException is not null)
            throw Retrieve()!;
    }

    private static void Set(Exception? exception)
    {
        if (_pendingException is not null)
            throw new ApplicationException(
                "FATAL: An earlier pending exception from "
                    + "unmanaged code was missed and thus not thrown ("
                    + _pendingException
                    + ")",
                exception
            );
        _pendingException = exception;
        lock (ExceptionsLock)
        {
            _pendingExceptionsCount++;
        }
    }

    private static Exception? Retrieve()
    {
        Exception? exception = null;
        if (_pendingExceptionsCount <= 0 || _pendingException is null)
            return exception;
        (exception, _pendingException) = (_pendingException, null);
        lock (ExceptionsLock)
        {
            _pendingExceptionsCount--;
        }

        return exception;
    }

    [DllImport(
        Library.Name,
        EntryPoint = "SWIGRegisterExceptionCallbacks_CoolProp"
    )]
    private static extern void RegisterExceptionCallbacks(
        ExceptionDelegate applicationDelegate,
        ExceptionDelegate arithmeticDelegate,
        ExceptionDelegate divideByZeroDelegate,
        ExceptionDelegate indexOutOfRangeDelegate,
        ExceptionDelegate invalidCastDelegate,
        ExceptionDelegate invalidOperationDelegate,
        ExceptionDelegate ioDelegate,
        ExceptionDelegate nullReferenceDelegate,
        ExceptionDelegate outOfMemoryDelegate,
        ExceptionDelegate overflowDelegate,
        ExceptionDelegate systemExceptionDelegate
    );

    [DllImport(
        Library.Name,
        EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_CoolProp"
    )]
    private static extern void RegisterExceptionArgumentCallbacks(
        ExceptionArgumentDelegate argumentDelegate,
        ExceptionArgumentDelegate argumentNullDelegate,
        ExceptionArgumentDelegate argumentOutOfRangeDelegate
    );

    private static void SetPendingApplicationException(string message) =>
        Set(new ApplicationException(message, Retrieve()));

    private static void SetPendingArithmeticException(string message) =>
        Set(new ArithmeticException(message, Retrieve()));

    private static void SetPendingDivideByZeroException(string message) =>
        Set(new DivideByZeroException(message, Retrieve()));

    private static void SetPendingIndexOutOfRangeException(string message) =>
        Set(new IndexOutOfRangeException(message, Retrieve()));

    private static void SetPendingInvalidCastException(string message) =>
        Set(new InvalidCastException(message, Retrieve()));

    private static void SetPendingInvalidOperationException(string message) =>
        Set(new InvalidOperationException(message, Retrieve()));

    private static void SetPendingIoException(string message) =>
        Set(new IOException(message, Retrieve()));

    private static void SetPendingNullReferenceException(string message) =>
        Set(new NullReferenceException(message, Retrieve()));

    private static void SetPendingOutOfMemoryException(string message) =>
        Set(new OutOfMemoryException(message, Retrieve()));

    private static void SetPendingOverflowException(string message) =>
        Set(new OverflowException(message, Retrieve()));

    private static void SetPendingSystemException(string message) =>
        Set(new SystemException(message, Retrieve()));

    private static void SetPendingArgumentException(
        string message,
        string argumentName
    ) => Set(new ArgumentException(message, argumentName, Retrieve()));

    private static void SetPendingArgumentNullException(
        string message,
        string argumentName
    )
    {
        var exception = Retrieve();
        if (exception is not null)
            message = message + " Inner Exception: " + exception.Message;
        Set(new ArgumentNullException(argumentName, message));
    }

    private static void SetPendingArgumentOutOfRangeException(
        string message,
        string argumentName
    )
    {
        var exception = Retrieve();
        if (exception is not null)
            message = message + " Inner Exception: " + exception.Message;
        Set(new ArgumentOutOfRangeException(argumentName, message));
    }

    private delegate void ExceptionArgumentDelegate(
        string message,
        string argumentName
    );

    private delegate void ExceptionDelegate(string message);
}
