using System.Net;

namespace ECommerceBem.Exception.ExceptionsBase;

public class NotFoundException(string message) : ECommerceBemException(message)
{
    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}