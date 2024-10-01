using System.Net;

namespace ECommerceBem.Exception.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errors) : ECommerceBemException(string.Empty)
{
    private readonly IList<string> _errors = errors;
    public override IList<string> GetErrorMessages()
    {
        return _errors;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}
