using Agora.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Agora.Application.DTOs.Errors
{
    public class ResponseSerializer(ILogger<ResponseSerializer> logger)
    {
        public ObjectResult ToActionResult(object? value)
        {
            return CreateResponse(HttpStatusCode.OK, new
            {
                SuccessResult = value
            });
        }

        public ObjectResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return CreateResponse(HttpStatusCode.OK, null);

            return SerializeErrorResult(result);
        }

        public ObjectResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return CreateResponse(HttpStatusCode.OK, new { SuccessResult = result.Value });

            return SerializeErrorResult(result);
        }

        private ObjectResult SerializeErrorResult(Result result)
        {
            return SerializeError(result.Error);
        }

        private ObjectResult SerializeError(Error? error)
        {
            if (error == null)
            {
                logger.LogError("Null error encountered");
                return InternalServerError();
            }

            // Customize specific error types as needed
            return error.Code switch
            {
                ErrorCodes.Unauthorized => CreateResponse(HttpStatusCode.Unauthorized, new ErrorResponse(error.Code, error.Message)),
                ErrorCodes.ServiceError => InternalServerError(),
                _ => CreateResponse(HttpStatusCode.BadRequest, new ErrorResponse(error.Code, error.Message))
            };
        }

        private ObjectResult CreateResponse(HttpStatusCode statusCode, object? value)
        {
            return new ObjectResult(value)
            {
                StatusCode = (int)statusCode
            };
        }

        private ObjectResult InternalServerError()
        {
            return CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(ErrorCodes.ServiceError, "Internal server error"));
        }
    }

    public static class ErrorCodes
    {
        public const string Unauthorized = "Unauthorized";
        public const string ServiceError = "ServiceError";
    }

    public class ErrorResponse
    {
        public ErrorResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }
    }
}
