namespace Entities.Exceptions;
public class NotFoundException : Exception, IErrorStatusCode
{
    public int GetErrorStatusCode() => 404;

    public NotFoundException(string message) : base(message)
    {
    }
}
