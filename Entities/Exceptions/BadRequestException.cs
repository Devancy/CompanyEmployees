namespace Entities.Exceptions;

public abstract class BadRequestException : Exception, IErrorStatusCode
{
    public int GetErrorStatusCode() => 400;
    protected BadRequestException(string message) : base(message)
    {
    }
}
