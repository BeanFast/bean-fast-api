using DataTransferObjects.Core.Response;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Utilities.Constants;
using Utilities.Exceptions;

namespace BeanFastApi.Middlewares
{
    public class ExceptionHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleWare> _logger;

        public ExceptionHandlingMiddleWare(RequestDelegate next, ILogger<ExceptionHandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorApiResponse();
            errorResponse.SetStatusCode(HttpStatusCode.InternalServerError);
            switch (exception)
            {
                case BeanFastApplicationException ex:

                    errorResponse.SetStatusCode(ex.StatusCode);
                    errorResponse.Message = ex.Message;
                    break;
                case ValidationException ex:
                    _logger.LogCritical("Invalid");
                    errorResponse.SetStatusCode(HttpStatusCode.BadRequest);
                    errorResponse.Message = ex.Message;
                    break;
                default: 
                    errorResponse.Message = exception.Message;
                    //errorResponse.Message = MessageContants.DefaultApiMessage.ApiError;
                    break;
            }
            _logger.LogError(exception?.Message);
            response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsJsonAsync(errorResponse);
            //errorResponse
        }
    }
}
