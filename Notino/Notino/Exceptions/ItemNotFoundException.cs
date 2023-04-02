namespace Notino.Exceptions;

public class ItemNotFoundException : NotinoException
{
    public Guid Id { get; }
    

    public ItemNotFoundException(Guid id) : base($"{id} was not found")
    {
        Id = id;
    }

    public ItemNotFoundException(Guid id, Exception? innerException) : base($"{id} was not found", innerException)
    {
        Id = id;
    }
}