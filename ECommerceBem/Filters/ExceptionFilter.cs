using ECommerceBem.Application.Dto.Response;
using ECommerceBem.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerceBem.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ECommerceBemException)
            {
                var ecommerceBemException = (ECommerceBemException)context.Exception;
                context.HttpContext.Response.StatusCode = (int)ecommerceBemException.GetStatusCode();
                var responseJson = new ResponseErrorsDto(ecommerceBemException.GetErrorMessages());
                context.Result = new NotFoundObjectResult(responseJson);
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var list = new List<string> { "Unknown errors" };
                var responseJson = new ResponseErrorsDto(list);
                context.Result = new ObjectResult(responseJson);
            }
        }
    }
}
