namespace Notino.Exceptions;

public class NotinoException : Exception
{
    public NotinoException()
    {
    }
    
    public NotinoException(string? message) : base(message)
    {
    }

    public NotinoException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}