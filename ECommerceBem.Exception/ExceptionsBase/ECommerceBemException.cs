using System.Net;

namespace ECommerceBem.Exception.ExceptionsBase;

public abstract class ECommerceBemException(string message) : SystemException(message)
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract IList<string> GetErrorMessages();
}
