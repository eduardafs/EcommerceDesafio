namespace ECommerceBem.Application.Dto.Response;

public class ResponseErrorsDto
{
    public IList<string> Errors { get; set; } = [];

    public ResponseErrorsDto(IList<string> errors)
    {
        Errors = errors;
    }
}
