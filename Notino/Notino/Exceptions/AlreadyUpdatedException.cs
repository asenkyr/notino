namespace Notino.Exceptions;

public class AlreadyUpdatedException : NotinoException
{
    public Guid Id { get; }
    

    public AlreadyUpdatedException(Guid id) : base($"{id} was already updated")
    {
        Id = id;
    }

    public AlreadyUpdatedException(Guid id, Exception? innerException) : base($"{id} was already updated", innerException)
    {
        Id = id;
    }
}